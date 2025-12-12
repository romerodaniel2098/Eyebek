using System;
using System.Linq;
using System.Threading.Tasks;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;
using Eyebek.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eyebek.Infrastructure.Seed;

public static class SuperAdminSeeder
{
    private const string SuperAdminEmail = "superadmin_company@eyebek.com";
    private const string SuperAdminUserEmail = "superadmin@eyebek.com";
    private const string DefaultPassword = "123456"; 

    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            // 1. Ensure SuperAdmin Company exists
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Email == SuperAdminEmail);
            bool companyCreated = false;

            if (company == null)
            {
                // Find ENTERPRISE plan or fallback to any plan/null
                var enterprisePlan = await context.Plans.FirstOrDefaultAsync(p => p.Category == "ENTERPRISE");
                
                company = new Company
                {
                    Name = "Eyebek SuperAdmin",
                    Phone = "+0000000000",
                    Email = SuperAdminEmail,
                    Address = "HQ",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultPassword), // Access credentials for company level if needed
                    Status = CompanyStatus.Activo,
                    CurrentUsers = 0,
                    PlanId = enterprisePlan?.Id, // Might be null if seeding failed differently, but assuming plans seeded
                    PlanStartDate = DateTime.UtcNow,
                    PlanEndDate = DateTime.UtcNow.AddYears(100), // "Quemado" / infinite
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();
                companyCreated = true;
                logger.LogInformation("SuperAdmin Company created.");
            }

            // 2. Ensure SuperAdmin User exists within that company
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == SuperAdminUserEmail);

            if (user == null)
            {
                user = new User
                {
                    CompanyId = company.Id, 
                    Name = "Super Admin User",
                    Email = SuperAdminUserEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultPassword),
                    Document = "00000000",
                    Role = UserRole.SuperAdmin, // Uses the new Enum value
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                
                // Update company user count if we just added one and it wasn't tracked
                if (!companyCreated) 
                {
                    // If we just created the company, count is 0, adding user makes it 1. 
                    // But if company existed and user didn't, we increment.
                    // Doing a raw count is safer to stay in sync.
                    var count = await context.Users.CountAsync(u => u.CompanyId == company.Id);
                    company.CurrentUsers = count;
                    context.Companies.Update(company);
                    await context.SaveChangesAsync();
                }
                else
                {
                    company.CurrentUsers = 1;
                    context.Companies.Update(company);
                    await context.SaveChangesAsync();
                }

                logger.LogInformation("SuperAdmin User created.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding SuperAdmin.");
            throw; 
        }
    }
}
