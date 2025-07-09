namespace Application.Mediator.Interface
{
    public interface IQueryMediator
    {
        Task<TResponse> Send<TEvent, TResponse>(TEvent request, CancellationToken cancellationToken) where TEvent : IEvent<TResponse>;
    }

}
