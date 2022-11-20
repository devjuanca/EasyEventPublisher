
using Domain;
using Domain.DomainEvents;
using EasyEventPublisher;
using EasyEventPublisher.Enum;
using EasyEventPublisher.Interfaces;
using EasyEventPublisherExample.Events.NotificationEvents;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterEvents(EventInjectingType.Singleton, typeof(Program));

builder.Services.RegisterEvents(EventInjectingType.Scoped, typeof(DummyClass));

var app = builder.Build();

app.MapPost("/test", async (IEventManager eventManager, CancellationToken cancellationToken) =>
{
    await eventManager.PublishAsync(new NotificationEvent { Date = DateTime.UtcNow, Message = "Message Test" }, paralelismDegree: 3, fireAndForget: true, cancellationToken);

    await eventManager.PublishAsync(new DomainEventExample { Id = new Random().Next(0, 100), Name = "SomeDomainEvent" }, paralelismDegree: 3, fireAndForget: false, cancellationToken);
});

app.Run();
