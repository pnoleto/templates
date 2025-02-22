using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Repositories;
using Infra.Database.ModelDbContext;
using Domain.Models;
using Infra.DI;

namespace Infra.Database.Tests
{
    public class ArticleRepositoryTests
    {
        private readonly IArticleRepository _articleRepository;
        private readonly NewsDbContext _context;
        public ArticleRepositoryTests()
        {
            ServiceProvider provider = new ServiceCollection()
            .AddInMemoryDbContext()
            .AddRepositories()
            .BuildServiceProvider();

            _context = provider.GetRequiredService<NewsDbContext>();

            _articleRepository = provider.GetRequiredService<IArticleRepository>(); 
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test, Order(1)]
        public void EnsureRepositoryBaseToBeDefined()
        {
            Assert.That(_articleRepository, Is.Not.Null);
        }

        [Test, Order(2)]
        public void EnsureArticleToBeInserted()
        {
            AddEntity();

            var entity = _articleRepository.Get(y=> true).Any();

            Assert.That(entity, Is.True);
        }

        [Test, Order(3)]
        public void EnsureArticleToBeUpdated()
        {
            var entity = _articleRepository.Get(y => true).First();

            entity.Active = true;

            _articleRepository.Update(entity);

            var entityExists = _articleRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test, Order(4)]
        public void EnsureArticleToBeDeleted()
        {
            var entity = _articleRepository.Get(y => y.Active == true).Single();

            _articleRepository.Delete(entity);

            var entityExists = _articleRepository.Get(y => true).Any();

            Assert.That(entityExists, Is.False);
        }

        private void AddEntity()
        {
            _articleRepository.Add(
                new Article
                {
                    Author = "",
                    Description = "Description",
                    Title = "Title",
                    Url = "",
                    UrlToImage = "",
                    Content = "",
                    PublishedAt = DateTime.Now,
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
