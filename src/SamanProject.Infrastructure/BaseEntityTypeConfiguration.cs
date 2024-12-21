using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamanProject.Domain;

namespace SamanProject.Infrastructure
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property<DateTimeOffset>("Created");
            builder.Property<DateTimeOffset?>("Updated");

            builder.Property<int>("Version").IsConcurrencyToken();
        }
    }
}
