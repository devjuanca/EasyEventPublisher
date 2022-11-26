using System.Threading;
using System.Threading.Tasks;

namespace EasyEventPublisher.Interfaces
{

    /// <summary>
    /// This service is used to publish events using method PublishAsync. 
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event">Event model defined.</param>
        /// <param name="paralelismDegree">Paralelism degree to execute event handlers, this is only used if fireAndForget is false.</param>
        /// <param name="fireAndForget">False: Wait for every handler to finish execution. True: Fire every event handler but doesn't wait execution to finish..</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task PublishAsync<T>(T @event, int paralelismDegree = 1, bool fireAndForget = false, CancellationToken cancellationToken = new CancellationToken()) where T : class;
    }
}
