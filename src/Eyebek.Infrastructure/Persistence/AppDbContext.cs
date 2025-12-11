using Eyebek.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eyebek.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets alineados con las tablas de la BD
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Session> Sessions => Set<Session>();

    // 🔹 Necesario para que compile AuditRepository
    // (aunque en la BD real no exista la tabla audits)
    public DbSet<Audit> Audits => Set<Audit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usamos nombres de tabla en minúscula como en la BD
        modelBuilder.Entity<Company>().ToTable("companies");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Plan>().ToTable("plans");
        modelBuilder.Entity<Payment>().ToTable("payments");
        modelBuilder.Entity<Attendance>().ToTable("attendance");
        modelBuilder.Entity<Session>().ToTable("sessions");

        // Índices
        modelBuilder.Entity<Company>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => new { x.CompanyId, x.Email }).IsUnique();

        // Enums como string
        modelBuilder.Entity<Company>().Property(x => x.Status).HasConversion<string>();
        modelBuilder.Entity<User>().Property(x => x.Role).HasConversion<string>();
        modelBuilder.Entity<User>().Property(x => x.Status).HasConversion<string>();

        modelBuilder.Entity<Payment>().Property(x => x.PaymentMethod).HasConversion<string>();
        modelBuilder.Entity<Payment>().Property(x => x.PaymentStatus).HasConversion<string>();

        modelBuilder.Entity<Attendance>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<Attendance>().Property(x => x.Method).HasConversion<string>();
    }
}
