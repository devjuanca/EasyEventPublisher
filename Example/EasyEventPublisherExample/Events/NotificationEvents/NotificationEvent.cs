using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents;

public class NotificationEvent : IEvent
{
    public string Message { get; set; } = default!;
    public DateTime Date { get; set; }
}
