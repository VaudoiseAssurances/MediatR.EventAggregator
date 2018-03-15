using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR.EventAggregator
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IMediator mediator;

        private readonly Dictionary<Type, Dictionary<object, object>> callbacks;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        /// <param name="mediator">
        /// The mediator instance to inject.
        /// </param>
        public EventAggregator(IMediator mediator)
        {
            this.mediator = mediator;
            this.callbacks = new Dictionary<Type, Dictionary<object, object>>();
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent ev)
        {
            var notification = ev as INotification;
            if (notification != null)
            {
                this.mediator.Publish(notification);
            }

            Task sendTask = null;
            var request = ev as IRequest;
            if (request != null)
            {
                sendTask = this.mediator.Send(request);
            }
      
            this.HandleCallbacks(ev);

            return sendTask ?? Task.CompletedTask;
        }

        // Note: can we make it more elegant to use , with better type inference ?
        /// <inheritdoc />
        public Task<TResponse> PublishAsync<TResponse, TEvent>(TEvent ev) 
            where TEvent : IRequest<TResponse>
        {
            var sendTask = this.mediator.Send(ev);

            this.HandleCallbacks(ev);

            return sendTask;
        }

        /// <inheritdoc cref="IEventDispatcher.Subscribe{TEvent}(System.Action{TEvent})"/>
        public void Subscribe<TEvent>(Action<TEvent> callback)
        {
            var type = this.AddNewTEventToCallbacks<TEvent>();

            this.callbacks[type].Add(callback, null);
        }

        /// <inheritdoc cref="IEventDispatcher.Subscribe{TEvent}(System.Action{TEvent})"/>
        public void Subscribe<TEvent>(Action<TEvent, object> callback, object state)
        {            
            var type = this.AddNewTEventToCallbacks<TEvent>();

            this.callbacks[type].Add(callback, state);
        }

        /// <inheritdoc cref="IEventDispatcher.Unsubscribe{TEvent}(System.Action{TEvent})"/>
        public void Unsubscribe<TEvent>(Action<TEvent> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            this.callbacks[typeof(TEvent)].Remove(callback);
        }

        /// <inheritdoc />
        public void Unsubscribe<TEvent>(Action<TEvent, object> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            this.callbacks[typeof(TEvent)].Remove(callback);
        }

        private Type AddNewTEventToCallbacks<TEvent>()
        {
            var type = typeof(TEvent);
            if (!this.callbacks.ContainsKey(type))
            {
                this.callbacks.Add(type, new Dictionary<object, object>());
            }

            return type;
        }

        private void HandleCallbacks<TEvent>(TEvent ev)
        {
            var eventType = typeof(TEvent);
            Dictionary<object, object> callbacksForEvent;
            var hasCallbacks = this.callbacks.TryGetValue(eventType, out callbacksForEvent);

            if (hasCallbacks)
            {
                foreach (var callbackInfos in callbacksForEvent.ToArray())
                {
                    var callback = callbackInfos.Key as Action<TEvent>;
                    if (callback != null)
                    {
                        callback(ev);
                    }
                    else
                    {
                        if (callbackInfos.Key is Action<TEvent, object> callbackWithState)
                        {
                            callbackWithState(ev, callbackInfos.Value);
                        }
                    }
                }
            }
        }
    }
}