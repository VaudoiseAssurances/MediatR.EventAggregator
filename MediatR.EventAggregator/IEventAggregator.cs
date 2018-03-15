namespace MediatR.EventAggregator
{
    /// <summary>
    /// An service providing with the event aggregator pattern.
    /// </summary>
    /// <seealso cref="MediatR.EventAggregator.IEventPublisher" />
    /// <seealso cref="MediatR.EventAggregator.IEventDispatcher" />
    /// <inheritdoc />
    public interface IEventAggregator : IEventPublisher, IEventDispatcher
    {
    }
}