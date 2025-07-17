using Application.DTO;
using Application.Mediator.Interface;
using Application.Results;

namespace Application.Handlers
{
    public class TestEventHandlerWithResult : IEventHandler<Test1, Result>
    {
        public ValueTask<Result> Execute(Test1 request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
