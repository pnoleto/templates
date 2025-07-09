using Application.DTO;
using Application.Mediator.Interface;

namespace Application.Handlers
{
    public class TestEventHandler : IEventHandler<Test>
    {
        public Task Execute(Test request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
