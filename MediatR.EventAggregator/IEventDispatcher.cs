using System;

namespace MediatR.EventAggregator
{
    /// <summary>
    /// A service providing mechanisms to subscribe to events.
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Subscribe a callback to an event
        /// </summary>
        /// <param name="callback">
        /// The callback to execute when event is triggered.
        /// </param>
        /// <typeparam name="TEvent">
        /// The event type
        /// </typeparam>
        void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IRequest;

        /// <summary>
        /// Unsubscribe an event callback
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="callback">the callback to remove</param>
        void Unsubscribe<TEvent>(Action<TEvent> callback) where TEvent : IRequest;

        /// <summary>
        /// Subscribe a callback to an event
        /// </summary>
        /// <param name="callback">
        /// The callback to execute when event is triggered.
        /// </param>
        /// <param name="state">
        /// The state of the callback
        /// </param>
        /// <typeparam name="TEvent">
        /// The event type
        /// </typeparam>
        void Subscribe<TEvent>(Action<TEvent, object> callback, object state) where TEvent : IRequest;

        /// <summary>
        /// Unsubscribe an event callback
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="callback">the callback to remove</param>
        void Unsubscribe<TEvent>(Action<TEvent, object> callback) where TEvent : IRequest;
    }
}