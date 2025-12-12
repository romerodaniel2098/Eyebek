using System;
using System.Linq;
using System.Threading.Tasks;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Seed;

public static class PlanSeeder
{
    // Versión síncrona (la usa Program.cs)
    public static void Seed(AppDbContext context)
    {
        // Si no se puede conectar, no hacemos nada
        if (!context.Database.CanConnect())
            return;
        
        // Ensure database is created (just in case, though Migrate should handle it)
        // Check safely if plans exist
        try 
        {
            if (context.Plans.Any())
                return;
        }
        catch 
        {
            // If table doesn't exist despite migration, we shouldn't crash, 
            // but normally Migrate() above should have created it.
            return;
        }

        // (Removed redundant check)

        var now = DateTime.UtcNow;

        var plans = new[]
        {
            new Plan
            {
                Category = "BÁSICO",
                Price = 0m,
                Duration = 30,
                Description = "Plan básico gratuito para pequeños equipos",
                UserCapacity = 5,
                Features = "Registro de asistencia, control de sesiones básicas",
                Active = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Plan
            {
                Category = "PRO",
                Price = 49_900m,
                Duration = 30,
                Description = "Plan PRO para empresas en crecimiento",
                UserCapacity = 50,
                Features = "Reportes avanzados, soporte prioritario",
                Active = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Plan
            {
                Category = "ENTERPRISE",
                Price = 199_900m,
                Duration = 30,
                Description = "Plan Enterprise para grandes compañías",
                UserCapacity = 500,
                Features = "Funciones avanzadas, integraciones externas",
                Active = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        context.Plans.AddRange(plans);
        context.SaveChanges();
    }

    // Versión asíncrona simple (1 parámetro)
    public static Task SeedAsync(AppDbContext context)
    {
        Seed(context);
        return Task.CompletedTask;
    }
    
    public static Task SeedAsync(AppDbContext context, object? _)
    {
        Seed(context);
        return Task.CompletedTask;
    }
}
