using Domain.Models;
using Infra.Database.ModelsMapping.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.ModelsMapping
{
    internal class SourceMapping : ModelBaseMapping<Source>
    {
        public override void Configure(EntityTypeBuilder<Source> builder)
        {
            base.Configure(builder);

            builder.Property(entity => entity.Name).IsRequired();
            builder.Property(entity => entity.Url).IsRequired();
            builder.HasIndex(entity => entity.Url).IsUnique();
            builder.Property(entity => entity.SKU).HasMaxLength(250).IsRequired();

            builder
              .HasMany(entity => entity.Articles)
              .WithOne(entity => entity.Source)
              .HasForeignKey(entity => entity.IdSource);
        }
    }
}
