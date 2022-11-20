using Domain.DomainEvents;
using EasyEventPublisher.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.DomainEventsHandler;

public class SecondDomainEvantHandler : IEventHandler<DomainEventExample>
{
    private readonly ILogger<SecondDomainEvantHandler> _logger;

    public SecondDomainEvantHandler(ILogger<SecondDomainEvantHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DomainEventExample @event, CancellationToken cancellationToken)
    {


        _logger.LogInformation($" {@event.Id} - {@event.Name} Executed by SecondDomainEvantHandler");

        return Task.CompletedTask;
    }
}
