using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Attendance;
using Eyebek.Application.Services.Interfaces;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("attendance")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;
    public AttendanceController(IAttendanceService service) => _service = service;

    // 6) Registrar asistencia (facial/manual)
    [HttpPost]
    public async Task<IActionResult> Register(AttendanceCreateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        await _service.RegisterAsync(companyId, request;
        return Ok(new { message = "Asistencia registrada." });
    }
}