using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
                
        builder.HasKey(doc => doc.Id);
        builder.Property(doc => doc.Number).IsRequired();
        
        builder.HasIndex(doc => doc.Number).IsUnique();
    }
}