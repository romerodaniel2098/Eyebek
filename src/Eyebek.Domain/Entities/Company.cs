using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Address { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public int? PlanId { get; set; }
    public Plan? Plan { get; set; }

    public DateTime? PlanStartDate { get; set; }
    public DateTime? PlanEndDate { get; set; }

    public CompanyStatus Status { get; set; } = CompanyStatus.SinPlan;

    public int CurrentUsers { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<User> Users { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
    public List<Session> Sessions { get; set; } = new();
    public List<Audit> Audits { get; set; } = new();
}