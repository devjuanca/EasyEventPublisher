using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyEventPublisher.Implementations;

internal class EventManager : IEventManager
{
    private readonly Dictionary<Type, Type> _dicc;

    private readonly IServiceScopeFactory _scopeFactory;

    public EventManager(IServiceDiccionary serviceDiccionary, IServiceScopeFactory scopeFactory)
    {
        _dicc = serviceDiccionary.ServiceKeyPairValues;
        _scopeFactory = scopeFactory;
    }

    public async Task PublishAsync<T>(T @event, int paralelismDegree = 1, bool fireAndForget = false, CancellationToken cancellationToken = new()) where T : class
    {
        var tasks = new List<Task>();

        var eventType = @event.GetType();

        var scope = _scopeFactory.CreateScope();

        _dicc.TryGetValue(eventType, out var handlerType);

        var services = scope.ServiceProvider.GetServices(handlerType!);

        foreach (var handlerService in services)
        {
            MethodInfo methodInfo = handlerService!.GetType().GetMethod("HandleAsync")!;

            var task = (Task)methodInfo!.Invoke(handlerService, new object[] { @event, cancellationToken })!;

            if (!fireAndForget)
                tasks.Add(task);
        }


        if (tasks.Any())
        {
            var chunckedTasks = tasks.Chunk(paralelismDegree);

            foreach (var chtasks in chunckedTasks)
            {
                await Task.WhenAll(chtasks);
            }
        }


    }
}
