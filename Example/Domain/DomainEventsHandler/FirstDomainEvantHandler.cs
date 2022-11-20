using Domain.DomainEvents;
using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.DomainEventsHandler
{
    public class FirstDomainEvantHandler : IEventHandler<DomainEventExample>
    {
        private readonly ILogger<FirstDomainEvantHandler> _logger;

        public FirstDomainEvantHandler(ILogger<FirstDomainEvantHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(DomainEventExample @event, CancellationToken cancellationToken)
        {
            await Task.Delay(5000);

            _logger.LogInformation($" {@event.Id} - {@event.Name} Executed by FirstDomainEvantHandler");

        }
    }
}
