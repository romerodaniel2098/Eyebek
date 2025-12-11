using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Payments;
using Eyebek.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentCreateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        if (companyId == null)
            return Unauthorized("No se encontró la empresa en el token.");
        
        await Task.CompletedTask;

        return Ok(new
        {
            message = "Pago registrado (implementación de servicio pendiente).",
            companyId = companyId.Value,
            payment = request
        });
    }

   
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var companyId = HttpContext.GetCompanyId();
        if (companyId == null)
            return Unauthorized("No se encontró la empresa en el token.");

     
        await Task.CompletedTask;

        var emptyList = new List<object>();

        return Ok(new
        {
            message = "Historial de pagos (implementación de servicio pendiente).",
            companyId = companyId.Value,
            payments = emptyList
        });
    }
}