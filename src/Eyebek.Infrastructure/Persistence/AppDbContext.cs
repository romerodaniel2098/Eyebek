using Eyebek.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eyebek.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tablas reales que tenemos en la BD
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // companies
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("companies");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Status).HasConversion<string>();
        });

        // users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();

            entity.Property(x => x.Role).HasConversion<string>();
            entity.Property(x => x.Status).HasConversion<string>();
        });

        // plans
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("plans");
            entity.HasKey(x => x.Id);
        });

        // payments
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.PaymentMethod).HasConversion<string>();
            entity.Property(x => x.PaymentStatus).HasConversion<string>();
        });

        // attendance
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("attendance");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).HasConversion<string>();
            entity.Property(x => x.Method).HasConversion<string>();
        });

        // sessions
        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("sessions");
            entity.HasKey(x => x.Id);
        });
    }
}
