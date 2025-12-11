using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class User
{
    public int Id { get; set; }

    // Relación con Company
    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    // Datos básicos
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Document { get; set; } = default!;
    

    // Rol y estado
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public string? Phone { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}