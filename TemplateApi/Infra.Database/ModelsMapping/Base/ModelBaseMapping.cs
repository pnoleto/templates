using Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Infra.Database.ModelsMapping.Generators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.ModelsMapping.Base
{
    internal class ModelBaseMapping<T> : BaseMapping<T>, IEntityTypeConfiguration<T> where T : ModelBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id)
                .IsRequired();
            builder.Property(entity => entity.CreatedAt)
                .HasValueGenerator<DateTimeGenerator>()
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(entity => entity.UpdatedAt)
                .HasValueGenerator<DateTimeGenerator>()
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();
            builder.Property(entity => entity.DeletedAt);
            builder.Property(entity => entity.Active)
                .HasDefaultValue(true)
                .IsRequired();
            builder.HasQueryFilter(entity => !entity.DeletedAt.HasValue && entity.Active);
        }
    }
}
