namespace Application.Mediator.Interface
{
    public interface IMediator
    {
        ValueTask Send<TEvent>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent;
        ValueTask<TResponse> Send<TEvent, TResponse>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent<TResponse>;
    }
}
