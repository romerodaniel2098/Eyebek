using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    // Estado y plan
    public CompanyStatus Status { get; set; }
    public int? PlanId { get; set; }
    public Plan? Plan { get; set; }

    public DateTime? PlanStartDate { get; set; }
    public DateTime? PlanEndDate { get; set; }

    // Usuarios actuales usando el sistema
    public int CurrentUsers { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}