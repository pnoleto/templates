namespace Application.Mediator.Interface
{
    public interface IEventHandlerBase
    {

    }

    public interface IEventHandler<in TEvent> : IEventHandlerBase where TEvent : IEvent
    {
        ValueTask Execute(TEvent request, CancellationToken cancellationToken);
    }

    public interface IEventHandler<in TEvent, TResult> : IEventHandlerBase where TEvent : IEvent<TResult>
    {
        ValueTask<TResult> Execute(TEvent request, CancellationToken cancellationToken);
    }
}
