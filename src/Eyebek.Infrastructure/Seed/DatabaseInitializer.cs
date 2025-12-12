using System;
using System.Threading.Tasks;
using Eyebek.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eyebek.Infrastructure.Seed;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(AppDbContext db, ILogger logger)
    {
        try
        {
            // 1. Crea las tablas si no existen (sin migraciones)
            await db.Database.EnsureCreatedAsync();

            // 2. Seed de planes (solo si la tabla está vacía)
            await PlanSeeder.SeedAsync(db, logger);

            // 3. Seed de SuperAdmin (quemado)
            await SuperAdminSeeder.SeedAsync(db, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al preparar la base de datos o seed de planes/superadmin");
        }
    }
}