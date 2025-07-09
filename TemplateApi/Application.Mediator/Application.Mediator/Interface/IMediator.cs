namespace Application.Mediator.Interface
{
    public interface IMediator
    {
        Task Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent;
        Task<TResponse> Send<TEvent, TResponse>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent<TResponse>;
    }
}
