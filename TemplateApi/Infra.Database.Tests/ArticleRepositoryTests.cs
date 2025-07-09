using Application.Mediator;
using Application.Mediator.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Database.Tests
{
    public class SourceRepositoryTests
    {
        private readonly IMediator _mediator;

        public SourceRepositoryTests()
        {
            ServiceProvider provider = new ServiceCollection()
                .AddSingleton<IMediator, Mediator>()
                .BuildServiceProvider();

            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test, Order(1)]
        public void EnsureRepositoryBaseToBeDefined()
        {
            Assert.That(_mediator, Is.Not.Null);
        }

    }
}
