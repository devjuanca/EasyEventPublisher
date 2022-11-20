using EasyEventPublisher.Interfaces;

namespace EasyEventPublisherExample.Events.NotificationEvents;

public class NotificationEvent
{
    public string Message { get; set; } = default!;
    public DateTime Date { get; set; }
}
