using EasyEventPublisher.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyEventPublisher.Interfaces;

public interface IEventManager
{
    Task PublishAsync(IEvent @event, int paralelismDegree = 1, bool fireAndForget = false, CancellationToken cancellationToken = new());
}
