using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data.Configurations;

public class MeasureUnitConfiguration : IEntityTypeConfiguration<MeasureUnit>
{
    public void Configure(EntityTypeBuilder<MeasureUnit> builder)
    {
        builder.HasKey(mu => mu.Id);
        builder.Property(mu => mu.Name).IsRequired();        
        
        builder.HasIndex(mu => mu.Name).IsUnique();
    }
}