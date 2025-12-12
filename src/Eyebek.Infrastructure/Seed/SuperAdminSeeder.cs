using System;
using System.Threading.Tasks;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;
using Eyebek.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eyebek.Infrastructure.Seed;

public static class SuperAdminSeeder
{
    // 👇 Coincide con appsettings.json -> "SuperAdmin"
    private const string SuperAdminCompanyEmail = "superadmin@eyebek.com";  
    private const string SuperAdminUserEmail    = "superadmin@eyebek.com";   
    private const string DefaultPassword        = "SuperAdmin123!";          

    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            // 1. Empresa SuperAdmin
            var company = await context.Companies
                .FirstOrDefaultAsync(c => c.Email == SuperAdminCompanyEmail);
            var companyCreated = false;

            if (company == null)
            {
                var enterprisePlan = await context.Plans
                    .FirstOrDefaultAsync(p => p.Category == "ENTERPRISE");

                company = new Company
                {
                    Name = "Super Administrador",
                    Phone = "+0000000000",
                    Email = SuperAdminCompanyEmail,
                    Address = "HQ",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultPassword),
                    Status = CompanyStatus.Activo,
                    CurrentUsers = 0,
                    PlanId = enterprisePlan?.Id,
                    PlanStartDate = DateTime.UtcNow,
                    PlanEndDate = DateTime.UtcNow.AddYears(100),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();
                companyCreated = true;

                logger.LogInformation("SuperAdmin company creada con email {Email}", SuperAdminCompanyEmail);
            }

            // 2. Usuario SuperAdmin dentro de esa empresa
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == SuperAdminUserEmail);

            if (user == null)
            {
                user = new User
                {
                    CompanyId = company.Id,
                    Name = "Super Administrador",
                    Email = SuperAdminUserEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultPassword),
                    Document = "00000000",
                    Role = UserRole.SuperAdmin,   // asegúrate de tener este valor en el enum
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                // Actualizar contador de usuarios de la empresa
                var count = await context.Users.CountAsync(u => u.CompanyId == company.Id);
                company.CurrentUsers = count;
                context.Companies.Update(company);
                await context.SaveChangesAsync();

                logger.LogInformation("SuperAdmin user creado con email {Email}", SuperAdminUserEmail);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding SuperAdmin.");
            throw;
        }
    }
}
