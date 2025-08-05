using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data;

public class WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : DbContext(options)
{
    DbSet<MeasureUnit> MeasureUnits { get; set; }
    DbSet<Resource> Resources { get; set; }
    DbSet<ResourceSupply> ResourceSupplies { get; set; }
    DbSet<Document> Documents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseDbContext).Assembly);
    }
}