using Eyebek.Domain.Enums;

namespace Eyebek.Application.DTOs.Payments;

public class PaymentCreateRequest
{
    public int PlanId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string PaymentReference { get; set; } = default!;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Approved;
}