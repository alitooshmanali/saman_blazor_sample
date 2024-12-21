using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SamanProject.Domain.Aggregates.Documents;
using SamanProject.Infrastructure.Context;

namespace SamanProject.Infrastructure.Aggregates.Documents.Configurations
{
    public class DocumentEntityTypeConfiguration: BaseEntityTypeConfiguration<Document>
    {
        public override void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable(nameof(SamanDbContext.Documents));

            builder.HasKey(d => d.Id);

            builder.Property(d => d.FileName)
                .IsRequired();

            builder.Property(d => d.FileContent)
                .IsRequired();

            builder.Property(d => d.FileExtension)
                .IsRequired();

            builder.Property(d => d.EntityType)
                .IsRequired();

            builder.Property(d => d.EntityId)
                .IsRequired();

            builder.Property(d => d.UploadedBy)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
