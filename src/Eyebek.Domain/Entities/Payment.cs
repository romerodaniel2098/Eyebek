using Eyebek.Domain.Enums;

namespace Eyebek.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public int PlanId { get; set; }
    public Plan Plan { get; set; } = default!;

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public string PaymentReference { get; set; } = default!;
    public string? Receipt { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}