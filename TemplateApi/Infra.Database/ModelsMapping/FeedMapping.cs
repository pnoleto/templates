using Domain.Models;
using Infra.Database.ModelsMapping.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.ModelsMapping
{
    internal class FeedMapping : ModelBaseMapping<Feed>
    {
        public override void Configure(EntityTypeBuilder<Feed> builder)
        {
            base.Configure(builder);

            builder.Property(entity=>entity.Name).IsRequired();
            builder.Property(entity => entity.Country).IsRequired();
            builder.Property(entity => entity.Category).IsRequired();
            builder.Property(entity => entity.Description).IsRequired();
            builder.Property(entity => entity.Url).IsRequired();
            builder.HasIndex(entity => entity.Url).IsUnique();

            builder.HasOne(entity => entity.Source)
                .WithMany(entity => entity.Feeds)
                .HasForeignKey(entity => entity.IdSource);
        }
    }
}
