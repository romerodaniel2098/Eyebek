using Microsoft.EntityFrameworkCore;
using Eyebek.Domain.Entities;

namespace Eyebek.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Audit> Audits => Set<Audit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();

        modelBuilder.Entity<Company>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<User>().Property(x => x.Role).HasConversion<string>();
        modelBuilder.Entity<User>().Property(x => x.Status).HasConversion<string>();

        modelBuilder.Entity<Payment>().Property(x => x.PaymentMethod).HasConversion<string>();
        modelBuilder.Entity<Payment>().Property(x => x.PaymentStatus).HasConversion<string>();

        modelBuilder.Entity<Attendance>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<Attendance>().Property(x => x.Method).HasConversion<string>();
    }
}   