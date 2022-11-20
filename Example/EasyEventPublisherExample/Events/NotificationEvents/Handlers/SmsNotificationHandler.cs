using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents.Handlers;

public class SmsNotificationHandler : IEventHandler<NotificationEvent>
{
    private readonly ILogger<SmsNotificationHandler> _logger;

    public SmsNotificationHandler(ILogger<SmsNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SMS Event Executed. Time: {time}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
