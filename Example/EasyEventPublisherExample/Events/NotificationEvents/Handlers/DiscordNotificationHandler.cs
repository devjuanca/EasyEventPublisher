using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents.Handlers
{
    public class DiscordNotificationHandler : IEventHandler<NotificationEvent>
    {
        private readonly ILogger<DiscordNotificationHandler> _logger;

        public DiscordNotificationHandler(ILogger<DiscordNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord Event Executed. Time: {time}", DateTime.UtcNow);

            return Task.CompletedTask;
        }
    }
}
