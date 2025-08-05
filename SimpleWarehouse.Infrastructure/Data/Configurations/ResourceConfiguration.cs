using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(re => re.Id);
        builder.Property(re => re.Name).IsRequired();
        
        builder.HasIndex(re => re.Name).IsUnique();
    }
}