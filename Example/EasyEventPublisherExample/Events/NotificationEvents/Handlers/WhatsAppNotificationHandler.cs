using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents.Handlers;

public class WhatsAppNotificationHandler : IEventHandler<NotificationEvent>
{
    private readonly ILogger<WhatsAppNotificationHandler> _logger;

    public WhatsAppNotificationHandler(ILogger<WhatsAppNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WhatsApp Event Executed. Time: {time}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
