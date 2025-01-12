using Domain.Models;
using Infra.Database.ModelsMapping.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.ModelsMapping
{
    internal class ArticleMapping : ModelBaseMapping<Article>
    {
        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            base.Configure(builder);

            builder.Property(entity => entity.Author).IsRequired();
            builder.Property(entity => entity.Content).IsRequired();
            builder.Property(entity => entity.Description).IsRequired();
            builder.Property(entity => entity.PublishedAt).IsRequired();
            builder.Property(entity => entity.UrlToImage).IsRequired();
            builder.Property(entity => entity.Url).IsRequired();
            builder.HasIndex(entity => entity.Url).IsUnique();

            builder.HasOne(entity => entity.Source)
                .WithMany(entity => entity.Articles)
                .HasForeignKey(entity => entity.IdSource);
        }
    }
}
