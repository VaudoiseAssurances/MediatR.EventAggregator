using System.Threading.Tasks;

namespace MediatR.EventAggregator
{
    /// <summary>
    /// A service providing mechanisms to publish events.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Asynchronously publishes the specified event in a fire-and-forget fashion.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="ev">The event.</param>
        /// <returns>A promise of completion</returns>
        Task PublishAsync<TEvent>(TEvent ev) where TEvent : IRequest;

        /// <summary>
        /// Publishes the specified event, expecting an asynchronous response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="ev">The event to publish.</param>
        /// <returns>
        /// A promise of a response to the published event.
        /// </returns>
        Task<TResponse> PublishAsync<TResponse, TEvent>(TEvent ev) where TEvent : IRequest<TResponse>; 
    }
}