namespace Application.Mediator.Interface
{
    public interface IEventHandlerBase
    {

    }

    public interface IEventHandler<in TEvent> : IEventHandlerBase where TEvent : IEvent
    {
        Task Execute(TEvent request, CancellationToken cancellationToken);
    }

    public interface IEventHandler<in TEvent, TResult> : IEventHandlerBase where TEvent : IEvent<TResult>
    {
        Task<TResult> Execute(TEvent request, CancellationToken cancellationToken);
    }
}
