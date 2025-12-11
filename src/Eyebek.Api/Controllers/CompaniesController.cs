using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Companies;
using Eyebek.Application.Services.Interfaces;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    // 1) Register company => sin_plan
    [HttpPost("register")]
    public async Task<ActionResult<CompanyMeResponse>> Register(CompanyRegisterRequest request)
    {
        var result = await _service.RegisterAsync(request);
        return Ok(result);
    }

    // 2) Login company => crea session + token
    [HttpPost("login")]
    public async Task<ActionResult> Login(CompanyLoginRequest request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();

        var (token, company) = await _service.LoginAsync(request, ip, ua);
        return Ok(new { token, company });
    }

    // GET perfil empresa autenticada
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CompanyMeResponse>> Me()
    {
        var companyId = HttpContext.GetCompanyId();
        var result = await _service.GetMeAsync(companyId);
        return Ok(result);
    }

    // CRUD Update básico de empresa (perfil)
    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<CompanyMeResponse>> UpdateMe(CompanyUpdateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        var result = await _service.UpdateMeAsync(companyId, request);
        return Ok(result);
    }
}