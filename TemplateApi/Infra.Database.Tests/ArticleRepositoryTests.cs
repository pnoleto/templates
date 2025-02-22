using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Infra.Database.ModelDbContext;
using Infra.DI;

namespace Infra.Database.Tests
{
    public class SourceRepositoryTests
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly NewsDbContext _context;
        public SourceRepositoryTests()
        {
            ServiceProvider provider = new ServiceCollection()
                .AddInMemoryDbContext()
                .AddRepositories()
                .BuildServiceProvider();

            _context = provider.GetRequiredService<NewsDbContext>();
            _sourceRepository = provider.GetRequiredService<ISourceRepository>();
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test, Order(1)]
        public void EnsureRepositoryBaseToBeDefined()
        {
            Assert.That(_sourceRepository, Is.Not.Null);
        }

        [Test, Order(2)]
        public void EnsureSourceToBeInserted()
        {
            AddEntity();

            var entity = _sourceRepository.Get(y=> true).Any();

            Assert.That(entity, Is.True);
        }

        [Test, Order(3)]
        public void EnsureSourceToBeUpdated()
        {
            Source entity = _sourceRepository.Get(y => y.Active == false).First();

            entity.Active = true;

            _sourceRepository.Update(entity);

            bool entityExists = _sourceRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test, Order(4)]
        public void EnsureSourceToBeDeleted()
        {
            Source entity = _sourceRepository.Get(y => y.Active == true).Single();

            _sourceRepository.Delete(entity);

            bool entityExists = _sourceRepository.Get(y => true).Any();

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
