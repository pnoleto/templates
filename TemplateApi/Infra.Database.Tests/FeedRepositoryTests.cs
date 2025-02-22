using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Repositories;
using Infra.Database.ModelDbContext;
using Domain.Models;
using Infra.DI;

namespace Infra.Database.Tests
{
    public class FeedRepositoryTests
    {
        private readonly IFeedRepository _feedRepository;
        private readonly NewsDbContext _context;
        public FeedRepositoryTests()
        {
            ServiceProvider provider = new ServiceCollection()
            .AddInMemoryDbContext()
            .AddRepositories()
            .BuildServiceProvider();

            _context = provider.GetRequiredService<NewsDbContext>();

            _feedRepository = provider.GetRequiredService<IFeedRepository>();
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test, Order(1)]
        public void EnsureRepositoryBaseToBeDefined()
        {
            Assert.That(_feedRepository, Is.Not.Null);
        }

        [Test, Order(2)]
        public void EnsureFeedToBeInserted()
        {
            AddEntity();

            var entity = _feedRepository.Get(y=> true).Any();

            Assert.That(entity, Is.True);
        }

        [Test, Order(3)]
        public void EnsureFeedToBeUpdated()
        {
            var entity = _feedRepository.Get(y => y.Active == false).First();

            entity.Active = true;

            _feedRepository.Update(entity);

            var entityExists = _feedRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test, Order(4)]
        public void EnsureFeedToBeDeleted()
        {
            var entity = _feedRepository.Get(y => y.Active == true).Single();

            _feedRepository.Delete(entity);

            var entityExists = _feedRepository.Get(y => true).Any();

            Assert.That(entityExists, Is.False);
        }

        private void AddEntity()
        {
            _feedRepository.Add(
                new Feed
                {
                    Name = "Test",
                    Url = "",
                    Description = "Test",
                    Country = "Test",
                    Category = "Test",
                    Language = "Test",
                    Active = false,
                });
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
