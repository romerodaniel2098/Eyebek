using Eyebek.Application.DTOs.Payments;

namespace Eyebek.Application.Services.Interfaces;

public interface IPaymentService
{
    Task CreateAndApplyPlanAsync(int companyId, PaymentCreateRequest request);
    Task<List<object>> HistoryAsync(int companyId);
}