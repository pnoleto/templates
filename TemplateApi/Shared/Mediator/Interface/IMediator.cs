namespace Shared.Mediator.Interface
{
    public interface IMediator
    {
        Task Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent;
        Task<TResponse> Send<TResponse>(IEvent<TResponse> request, CancellationToken cancellationToken) where TResponse: class;
    }

}
