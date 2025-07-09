using Application.Mediator.Interface;

namespace Application.Mediator
{
    public abstract class EventHandlerBase
    {
        public abstract object? Execute(object request, CancellationToken cancellationToken);
    }
}
