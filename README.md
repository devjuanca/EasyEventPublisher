# EventPublisher
This lightweight library allows you to publish events and define as many handlers as you need. It is very simple to use as defined in this example:

```
public class NotificationEvent : IEvent
{
 public string Message {get;set}
 public DateTime Date {get;set;}
}


public class WhatsAppEventHandler : IEventHandler<NotificationEvent>
{
    private ILogger<WhatsAppEventHandler> _logger { get; set; }

    public WhatsAppEventHandler(ILogger<WhatsAppEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        ...
        _logger.LogInformation("Whatsapp Sent");
    }
}

public class EmailEventHandler : IEventHandler<NotificationEvent>
{
    private ILogger<EmailEventHandler> _logger;

    public EmailEventHandler(ILogger<EmailEventHandler> logger)
    {
        _logger = logger;
    }



    public async Task HandleAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        ...
        _logger.LogInformation("Email Sent");
    }
}


```

In Program.cs or using an extension method of ICollectionService, events should be registered as follows:
```
services.RegisterEvents();
```
RegisterEvents() allows a parameter of type EventInjectingType which is used to specify how events handlers should be registered in DI container, the possible values are:
Singleton, Scoped, Transcient.

By default EventInjectingType.Singleton will be used, however if you intend to inject in the event handler counstructor any scoped services (as DbContext) then the handlers should be registered as EventInjectingType.Scoped

To publish events IEventManager must be injected wherever you want to publish the event like this:

```
public class CreateNewProductService : ICreateNewProductService
{
  private readonly IEventManager _eventManager;
  
  public CreateNewProduct(IEventManager eventManager)
  {
   _eventManager = eventManager;
  }
  
  public Task CreateNewProduct(ProductDto product, CancellationToken ctx)
  {
    //Add new product
    ...
    _eventManager.PublishAsync(new NotificationEvent
    {
        Date = DateTime.Now,
        Message = "New Product was added."
    },
    fireAndForget: true, paralelismDegree: 2);
  }
}
```
PublichAsync() parameters are:
1. Event model: Entity of previosly defined class implementing IEvent.
2. fireAndForget (boolean) : If process must await for all events handlers to finish or not.
3. paralelismDegree: Degree of parallelism in which the handlers must execute
