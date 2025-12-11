using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Seed;

public static class PlanSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Plans.Any()) return;

        db.Plans.AddRange(
            new Plan
            {
                Category = "Basic",
                Price = 30000,
                Duration = 30,
                Description = "Plan básico para equipos pequeños",
                UserCapacity = 5,
                Active = true
            },
            new Plan
            {
                Category = "Pro",
                Price = 70000,
                Duration = 30,
                Description = "Plan profesional con mayor capacidad",
                UserCapacity = 25,
                Active = true
            },
            new Plan
            {
                Category = "Enterprise",
                Price = 150000,
                Duration = 30,
                Description = "Plan empresarial",
                UserCapacity = 200,
                Active = true
            }
        );

        await db.SaveChangesAsync();
    }
}