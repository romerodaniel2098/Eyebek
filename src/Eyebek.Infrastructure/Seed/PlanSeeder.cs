using System.Linq;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Seed;

public static class PlanSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Si ya hay planes, no hacemos nada
        if (context.Plans.Any())
            return;

        var plans = new List<Plan>
        {
            new Plan
            {
                Category = "Basic",
                Price = 30000,
                Description = "Plan básico para equipos pequeños (hasta 5 usuarios).",
                UserCapacity = 5
            },
            new Plan
            {
                Category = "Pro",
                Price = 80000,
                Description = "Plan Pro para pymes (hasta 20 usuarios).",
                UserCapacity = 20
            },
            new Plan
            {
                Category = "Enterprise",
                Price = 150000,
                Description = "Plan Enterprise para empresas grandes (hasta 50 usuarios).",
                UserCapacity = 50
            }
        };

        context.Plans.AddRange(plans);
        context.SaveChanges();
    }
}