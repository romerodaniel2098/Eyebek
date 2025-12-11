using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string Document { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? Photo { get; set; }

    public UserRole Role { get; set; } = UserRole.Employee;
    public UserStatus Status { get; set; } = UserStatus.Active;
    
    public string? FacialEmbedding { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Attendance> Attendances { get; set; } = new();
    public List<Audit> Audits { get; set; } = new();
}