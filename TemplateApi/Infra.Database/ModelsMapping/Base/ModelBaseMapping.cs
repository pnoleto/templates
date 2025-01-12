using Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.ModelsMapping.Base
{
    internal class ModelBaseMapping<T> : BaseMapping<T>, IEntityTypeConfiguration<T> where T : ModelBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(entity => entity.CreatedAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(entity => entity.UpdatedAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(entity => entity.DeletedAt).HasDefaultValue(null);
            builder.Property(entity => entity.Active).HasDefaultValue(true).IsRequired();

        }
    }
}
