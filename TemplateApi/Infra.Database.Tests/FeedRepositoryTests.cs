using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Tests
{
    public class FeedRepositoryTests
    {
        private readonly IFeedRepository _feedRepository;
        private readonly NewsDbContext _context;
        public FeedRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new NewsDbContext(options);

            _feedRepository = new FeedRepository(_context);
        }

        [SetUp]
        public void SetUp()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void EnsureRepositoryBaseToBeDefined()
        {
            Assert.That(_feedRepository, Is.Not.Null);
        }

        [Test]
        public void EnsureFeedToBeInserted()
        {
            AddEntity();

            var entity = _feedRepository.Get(y=> y != null).Any();

            Assert.That(entity, Is.True);
        }

        [Test]
        public void EnsureFeedToBeUpdated()
        {
            AddEntity();

            var entity = _feedRepository.Get(y => y != null).First();

            entity.Active = true;

            _feedRepository.Update(entity);

            var entityExists = _feedRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test]
        public void EnsureFeedToBeDeleted()
        {
            AddEntity();

            var entity = _feedRepository.Get(y => y.Active == false).Single();

            _feedRepository.Delete(entity);

            var entityExists = _feedRepository.Get(y => y.Active == false).Any();

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
