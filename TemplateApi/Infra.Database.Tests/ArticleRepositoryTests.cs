using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Tests
{
    public class SourceRepositoryTests
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly NewsDbContext _context;
        public SourceRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new NewsDbContext(options);

            _sourceRepository = new SourceRepository(_context);
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
            Assert.That(_sourceRepository, Is.Not.Null);
        }

        [Test]
        public void EnsureSourceToBeInserted()
        {
            AddEntity();

            var entity = _sourceRepository.Get(y=> true).Any();

            Assert.That(entity, Is.True);
        }

        [Test]
        public void EnsureSourceToBeUpdated()
        {
            AddEntity();

            var entity = _sourceRepository.Get(y => true).First();

            entity.Active = true;

            _sourceRepository.Update(entity);

            var entityExists = _sourceRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test]
        public void EnsureSourceToBeDeleted()
        {
            AddEntity();

            var entity = _sourceRepository.Get(y => y.Active == false).Single();

            _sourceRepository.Delete(entity);

            var entityExists = _sourceRepository.Get(y => y.Active == false).Any();

            Assert.That(entityExists, Is.False);
        }

        private void AddEntity()
        {
            _sourceRepository.Add(
                new Source
                {
                    Name = "Test",
                    Url = "Test",
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
