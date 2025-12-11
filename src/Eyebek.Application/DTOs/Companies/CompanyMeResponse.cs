using Eyebek.Domain.Enums;

namespace Eyebek.Application.DTOs.Companies;

public class CompanyMeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public CompanyStatus Status { get; set; }

    public int? PlanId { get; set; }
    public DateTime? PlanStartDate { get; set; }
    public DateTime? PlanEndDate { get; set; }

    public int CurrentUsers { get; set; }
}