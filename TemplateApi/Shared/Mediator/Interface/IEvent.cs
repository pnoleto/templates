namespace Shared.Mediator.Interface
{
    public interface IEventBase
    {
    }

    public interface IEvent : IEventBase
    {
    }

    public interface IEvent<TResult>: IEventBase
    {
    }
}
