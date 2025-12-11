using System.Collections.Generic;
using System.Threading.Tasks;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eyebek.Infrastructure.Seed;

public static class PlanSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        try
        {
            // Si ya hay planes, no hacemos nada
            if (await db.Plans.AnyAsync())
                return;

            var plans = new List<Plan>
            {
                new()
                {
                    Category = "Free",
                    Price = 0m,
                    Duration = 30,
                    Description = "Plan gratuito para probar Eyebek.",
                    UserCapacity = 5,
                    Features = "Registros básicos de asistencia",
                    Active = true
                },
                new()
                {
                    Category = "Pro",
                    Price = 29m,
                    Duration = 30,
                    Description = "Plan para pequeñas empresas.",
                    UserCapacity = 25,
                    Features = "Más usuarios, exportaciones y reportes.",
                    Active = true
                },
                new()
                {
                    Category = "Enterprise",
                    Price = 99m,
                    Duration = 30,
                    Description = "Plan para empresas grandes.",
                    UserCapacity = 100,
                    Features = "Todas las funcionalidades + soporte prioritario.",
                    Active = true
                }
            };

            db.Plans.AddRange(plans);
            await db.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error al hacer seed de planes");
        }
    }
}