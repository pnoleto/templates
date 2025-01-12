using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Database.ModelDbContext;
using Infra.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Tests
{
    public class ArticleRepositoryTests
    {
        private readonly IArticleRepository _articleRepository;
        private readonly NewsDbContext _context;
        public ArticleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new NewsDbContext(options);

            _articleRepository = new ArticleRepository(_context);
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
            Assert.That(_articleRepository, Is.Not.Null);
        }

        [Test]
        public void EnsureArticleToBeInserted()
        {
            AddEntity();

            var entity = _articleRepository.Get(y=> y != null).Any();

            Assert.That(entity, Is.True);
        }

        [Test]
        public void EnsureArticleToBeUpdated()
        {
            AddEntity();

            var entity = _articleRepository.Get(y => y != null).First();

            entity.Active = true;

            _articleRepository.Update(entity);

            var entityExists = _articleRepository.Get(y => y.Active == true).Any();

            Assert.That(entityExists, Is.True);
        }

        [Test]
        public void EnsureArticleToBeDeleted()
        {
            AddEntity();

            var entity = _articleRepository.Get(y => y.Active == false).Single();

            _articleRepository.Delete(entity);

            var entityExists = _articleRepository.Get(y => y.Active == false).Any();

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
