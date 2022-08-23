
using EasyEventPublisher.Implementations;
using EasyEventPublisher.Interfaces;
using EasyEventPublisherExample.Events.NotificationEvents;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterEvents();

var app = builder.Build();

app.MapPost("/test", async (IEventManager eventManager, CancellationToken cancellationToken) => 
{
   await eventManager.PublishAsync(new NotificationEvent { Date = DateTime.UtcNow, Message ="Message Test" }, paralelismDegree: 3, fireAndForget: true, cancellationToken);
});

app.Run();
