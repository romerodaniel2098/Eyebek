using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Payments;
using Eyebek.Application.Services.Interfaces;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    public PaymentsController(IPaymentService service) => _service = service;

    // 4) Pagar plan => activa suscripción
    [HttpPost]
    public async Task<IActionResult> Pay(PaymentCreateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        await _service.CreateAndApplyPlanAsync(companyId, request);
        return Ok(new { message = "Pago registrado y plan aplicado si fue aprobado." });
    }

    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var companyId = HttpContext.GetCompanyId();
        var result = await _service.HistoryAsync(companyId);
        return Ok(result);
    }
}