using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Infrastructure.Data;

namespace SimpleWarehouse.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
        dbContext.Database.Migrate();
    }
}