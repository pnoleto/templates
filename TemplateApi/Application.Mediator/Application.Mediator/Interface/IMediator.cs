namespace Application.Mediator.Interface
{
    public interface IMediator
    {
        Task Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
