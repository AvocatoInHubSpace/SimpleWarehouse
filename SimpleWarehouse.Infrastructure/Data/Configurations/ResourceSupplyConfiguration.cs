using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data.Configurations;

public class ResourceSupplyConfiguration : IEntityTypeConfiguration<ResourceSupply>
{
    public void Configure(EntityTypeBuilder<ResourceSupply> builder)
    {
        builder.HasKey(rs => rs.Id);
        builder.Property(rs => rs.Quantity).IsRequired();
        builder.Property(rs => rs.DocumentId).IsRequired();
        builder.Property(rs => rs.ResourceId).IsRequired();
        builder.Property(rs => rs.MeasureUnitId).IsRequired();
        
        builder.ToTable(t => t.HasCheckConstraint("CK_ResourceSupply_Quantity", "\"Quantity\"> 0"));
    }
}