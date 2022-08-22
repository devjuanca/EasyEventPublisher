using EventPublisher.Enum;
using EventPublisher.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventPublisher.Implementations;

public static class EventsExtension
{
    public static IServiceCollection RegisterEvents(this IServiceCollection services, EventInjectingType eventInyectingType = EventInjectingType.Singleton)
    {
        var eventsDicc = new Dictionary<Type, Type>();

        var events = Assembly.GetEntryAssembly()!.ExportedTypes.Where(a => typeof(IEvent).IsAssignableFrom(a) && !a.IsInterface && !a.IsAbstract).ToList();

        events.AddRange(Assembly.GetCallingAssembly()!.ExportedTypes.Where(a => typeof(IEvent).IsAssignableFrom(a) && !a.IsInterface && !a.IsAbstract).ToList());

        foreach (var @eventType in events)
        {
            var handlersTypes = Assembly.GetEntryAssembly()!.ExportedTypes
                .Where(a => a.GetInterfaces().Where(a => a.IsGenericType).Any(a => a.GetGenericArguments()[0] == eventType))
                .ToList();

            handlersTypes.AddRange(Assembly.GetCallingAssembly()!.ExportedTypes
                .Where(a => a.GetInterfaces().Where(a => a.IsGenericType).Any(a => a.GetGenericArguments()[0] == eventType))
                .ToList());

            if (!handlersTypes.Any())
            {
                throw new Exception($"Event {eventType.Name} has no handlers defined. At least one handler must be defined in every IEvent.");
            }


            foreach (var item in handlersTypes)
            {
                var service = item.GetInterfaces().Where(a => a.IsGenericType).Where(a => a.GetGenericArguments()[0] == eventType).FirstOrDefault();

                switch (eventInyectingType)
                {
                    case EventInjectingType.Singleton:
                        {
                            services.AddSingleton(service!, item);
                        }
                        break;
                    case EventInjectingType.Scoped:
                        {
                            services.AddScoped(service!, item);
                        }
                        break;
                    case EventInjectingType.Transcient:
                        {
                            services.AddTransient(service!, item);
                        }
                        break;
                }
            }



            eventsDicc.Add(eventType, handlersTypes.First().GetInterfaces().Where(a => a.IsGenericType).Where(a => a.GetGenericArguments()[0] == eventType).FirstOrDefault()!);
        }

        services.AddSingleton(eventsDicc);

        services.AddSingleton<IEventManager, EventManager>();

        return services;
    }
}
