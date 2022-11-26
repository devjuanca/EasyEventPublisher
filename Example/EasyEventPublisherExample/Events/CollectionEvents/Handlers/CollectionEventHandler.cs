using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.CollectionEvents.Handlers;

public class CollectionEventHandler : IEventHandler<List<string>>
{
    private readonly ILogger<CollectionEventHandler> _logger;

    public CollectionEventHandler(ILogger<CollectionEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(List<string> @event, CancellationToken cancellationToken)
    {
        foreach (var item in @event)
        {
            _logger.LogInformation(item);
        }

        return Task.CompletedTask;
    }
}
