namespace Application.Mediator.Interface
{
    public interface IEventBase
    {
    }

    public interface IEvent: IEventBase
    {
    }

    public interface IEvent<out TResult>
    {
    }
}
