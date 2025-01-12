using Domain.Models;
using Domain.Models.Base;

namespace Domain.Tests.Models
{
    public class ModelsTest
    {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void EnsureModelBaseToBeDefined()
        {
            Assert.That(new ModelBase
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
                Active = true
            }, Is.Not.Null);
        }

        [Test]
        public void EnsureArticleToBeDefined()
        {
            Assert.That(new Article
            {
                Author = "",
                Content = "",
                PublishedAt = DateTime.Now,
                Source = new Source
                {
                    Name = "",
                    Url = ""
                },
                Description = "",
                UrlToImage = "",
                Url = "",
                Title = "",
            }, Is.Not.Null);
        }

        [Test]
        public void EnsureFeedToBeDefined()
        {
            Assert.That(new Feed
            {
                Description = "description",
                Url = "url",
                Country = "",
                Language = "en",
                Name = "name"
            }, Is.Not.Null);
        }

        [Test]
        public void EnsureSourceToBeDefined()
        {
            Assert.That(new Source
            {
                Name = "name",
                Url = "url",
            }, Is.Not.Null);
        }

    }

}
