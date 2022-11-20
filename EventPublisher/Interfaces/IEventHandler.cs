namespace EasyEventPublisher.Interfaces;

/// <summary>
/// This interface must be implemented to define a handler for an event.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
