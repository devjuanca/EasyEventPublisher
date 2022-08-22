using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyEventPublisher.Implementations;

public class EventManager : IEventManager
{
    private readonly Dictionary<Type, Type> _dicc;

    private readonly IServiceScopeFactory _scopeFactory;

    public EventManager(Dictionary<Type, Type> dicc, IServiceScopeFactory scopeFactory)
    {
        _dicc = dicc;
        _scopeFactory = scopeFactory;
    }

    public async Task PublishAsync(IEvent @event, int paralelismDegree = 1, bool fireAndForget = false, CancellationToken cancellationToken = new())
    {
        var tasks = new List<Task>();

        var eventType = @event.GetType();

        var scope = _scopeFactory.CreateScope();

        //Get all handlers for the event

        _dicc.TryGetValue(eventType, out var handlerType);

        var services = scope.ServiceProvider.GetServices(handlerType!);

        foreach (var handlerService in services)
        {
            var type = handlerService!.GetType();

            MethodInfo methodInfo = type.GetMethod("HandleAsync")!;

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
