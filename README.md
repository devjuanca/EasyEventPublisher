# EasyEventPublisher
This lightweight library allows you to publish events and define as many handlers as you need. It is very simple to use as defined in this example:

Install package:
```
Install-Package EasyEventPublisher -Version 1.0.3
```

1. Define your events models
```
public class NotificationEvent
{
 public string Message {get; set}
 public DateTime Date {get; set;}
}
```
2. Use ```IEventHandler<T>``` interface to implement event handlers that will be executed once yo publish an event of type T (class).
```
public class WhatsAppEventHandler : IEventHandler<NotificationEvent>
{
    private ILogger<WhatsAppEventHandler> _logger;

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

3. Use extension method of RegisterEvents to automatically register each event handler found in defined assembly

RegisterEvents() has two parameters: EventInjectingType which is used to specify how events handlers should be registered in DI container, the possible values are:
(Singleton, Scoped, Transcient) and params Type[] handlerAssemblyMarkerTypes which are types from assemblies where Event Handlers should me searched.

```
var builder = WebApplication.CreateBuilder();

// In this case we are registering events handlers from two different assemblies and two differents InjectingType.
// In case EventInjectingType value is the same it can be done like this:
// builder.Services.RegisterEvents(EventInjectingType.Singleton, typeof(Program), typeof(DummyClass));

builder.Services.RegisterEvents(EventInjectingType.Singleton, typeof(Program));

builder.Services.RegisterEvents(EventInjectingType.Scoped, typeof(DummyClass));

var app = builder.Build();

app.Run();

```

By default EventInjectingType.Singleton will be used, however if you intend to inject in the event handler counstructor any scoped services (as DbContext) then the handlers should be registered as EventInjectingType.Scoped

To publish events IEventManager must be injected wherever you want to publish the event just like this:

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
    fireAndForget: true, paralelismDegree: 2, ctx);
  }
}
```
PublichAsync() parameters are:
1. event model:  Previosly event model class defined.
2. fireAndForget (boolean) : If defines the behavior of events handlers, wait for all to finish execution or fire and forget.
3. paralelismDegree: This make sense if fireAndForget is false. It defines the parallelism degree of handlers execution.
4. cancellation token.
