using Shared.Mediator.Interface;

namespace Shared.Mediator.Base
{
    public abstract class EventHandlerBase<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        public Task Execute(TEvent request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class EventHandlerBase<TEvent, TResult> : IEventHandler<TEvent, TResult> where TEvent : IEvent<TResult>
    {
        public Task<TResult> Execute(TEvent request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
