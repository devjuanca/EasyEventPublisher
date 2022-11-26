using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EasyEventPublisher.Implementations
{

    internal class EventManager : IEventManager
    {
        private readonly Dictionary<Type, Type> _dicc;

        private readonly IServiceScopeFactory _scopeFactory;

        public EventManager(IServiceDiccionary serviceDiccionary, IServiceScopeFactory scopeFactory)
        {
            _dicc = serviceDiccionary.ServiceKeyPairValues;
            _scopeFactory = scopeFactory;
        }

        public async Task PublishAsync<T>(T @event, int paralelismDegree = 1, bool fireAndForget = false, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            var tasks = new List<Task>();

            var eventType = @event.GetType();

            var scope = _scopeFactory.CreateScope();

            _dicc.TryGetValue(eventType, out Type handlerType);

            var services = scope.ServiceProvider.GetServices(handlerType).ToArray();

            for (int i = 0; i < services.Length; i++)
            {
                MethodInfo methodInfo = services[i].GetType().GetMethod("HandleAsync");

                var task = (Task)methodInfo.Invoke(services[i], new object[] { @event, cancellationToken });

                if (!fireAndForget)
                    tasks.Add(task);
            }


            if (tasks.Any())
            {
                var chunckedTasks = tasks.Chunk(paralelismDegree);

                for (int i = 0; i < chunckedTasks.Length; i++)
                {
                    await Task.WhenAll(chunckedTasks[i]);
                }

            }


        }

    }



}
