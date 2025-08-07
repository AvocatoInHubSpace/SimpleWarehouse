using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Data;

public class WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : DbContext(options)
{
    public DbSet<MeasureUnit> MeasureUnits { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceSupply> ResourceSupplies { get; set; }
    public DbSet<Document> Documents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseDbContext).Assembly);
    }
}