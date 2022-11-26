using EasyEventPublisher.Enum;
using EasyEventPublisher.Implementations;
using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyEventPublisher
{

    public static class EventsExtension
    {
        /// <summary>
        /// Extension method to register all events found by assembly. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="eventInyectingType">Defines how event handlers are registered, Singleton, Scoped or Transcient.</param>
        /// <param name="handlerAssemblyMarkerTypes">Type marker for each assembly where events must be searched.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection RegisterEvents(this IServiceCollection services, EventInjectingType eventInyectingType = EventInjectingType.Singleton, params Type[] handlerAssemblyMarkerTypes)
        {
            var eventsDicc = new Dictionary<Type, Type>();

            var eventAndHandlers = new List<(Type eventType, Type handlerType)>();

            foreach (var markerType in handlerAssemblyMarkerTypes)
            {
                var assembly = Assembly.GetAssembly(markerType) ?? throw new Exception("Assembly for this type does not exist");

                var eventsHandlers = assembly.DefinedTypes.Where(a => a.GetInterfaces().Select(b => b.Name).Contains("IEventHandler`1") && !a.IsInterface && !a.IsAbstract).ToList();

                foreach (var handler in eventsHandlers)
                {
                    var eventInterface = handler.GetInterfaces().FirstOrDefault(a => a.Name == "IEventHandler`1");

                    if (eventInterface is null)
                        throw new Exception("Events handlers must implement IEventHandler<TEvent>");

                    var eventModel = eventInterface.GetGenericArguments().First();

                    switch (eventInyectingType)
                    {
                        case EventInjectingType.Singleton:
                            {
                                services.AddSingleton(eventInterface, handler);
                            }
                            break;
                        case EventInjectingType.Scoped:
                            {
                                services.AddScoped(eventInterface, handler);
                            }
                            break;
                        case EventInjectingType.Transcient:
                            {
                                services.AddTransient(eventInterface, handler);
                            }
                            break;
                    }
                    if (!eventsDicc.ContainsKey(eventModel))
                        eventsDicc.Add(eventModel, eventInterface);
                }
            }

            var existingDiccService = services.FirstOrDefault(a => a.ServiceType == typeof(IServiceDiccionary));

            if (existingDiccService is null)
                services.AddSingleton<IServiceDiccionary>(new ServiceDiccionary { ServiceKeyPairValues = eventsDicc });
            else
            {
                var registeredDicc = (existingDiccService.ImplementationInstance as ServiceDiccionary).ServiceKeyPairValues;

                foreach (var item in registeredDicc)
                {
                    eventsDicc.Add(item.Key, item.Value);
                }

                services.AddSingleton<IServiceDiccionary>(new ServiceDiccionary { ServiceKeyPairValues = eventsDicc });
            }

            services.AddSingleton<IEventManager, EventManager>();

            return services;
        }
    }
}
