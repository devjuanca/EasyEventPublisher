using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents.Handlers;

public class EmailNotificationHandler : IEventHandler<NotificationEvent>
{
    private readonly ILogger<EmailNotificationHandler> _logger;

    public EmailNotificationHandler(ILogger<EmailNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email Event Executed. Time: {time}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
