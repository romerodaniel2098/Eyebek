using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Companies;
using Eyebek.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Registro de empresa (sin plan).
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CompanyRegisterRequest request)
    {
        var company = await _companyService.RegisterAsync(request);
        return Ok(company);
    }

    /// <summary>
    /// Login de empresa. Devuelve token JWT y datos básicos.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] CompanyLoginRequest request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        var (token, company) = await _companyService.LoginAsync(request, ip, userAgent);

        return Ok(new
        {
            token,
            company
        });
    }

    /// <summary>
    /// Datos de la empresa autenticada.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var companyId = HttpContext.GetCompanyId();
        if (companyId == null)
            return Unauthorized("No se encontró la empresa en el token.");

        var company = await _companyService.GetMeAsync(companyId.Value);
        return Ok(company);
    }

    /// <summary>
    /// Actualizar datos básicos de la empresa autenticada.
    /// </summary>
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] CompanyUpdateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        if (companyId == null)
            return Unauthorized("No se encontró la empresa en el token.");

        var company = await _companyService.UpdateMeAsync(companyId.Value, request);
        return Ok(company);
    }
}
