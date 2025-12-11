using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eyebek.Api.Helpers;
using Eyebek.Application.DTOs.Users;
using Eyebek.Application.Services.Interfaces;

namespace Eyebek.Api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    // 5) Agregar usuarios hasta el límite del plan
    [HttpPost]
    public async Task<ActionResult<UserListItemDto>> Create(UserCreateRequest request)
    {
        var companyId = HttpContext.GetCompanyId();
        var result = await _service.CreateAsync(companyId, request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserListItemDto>>> Get()
    {
        var companyId = HttpContext.GetCompanyId();
        var result = await _service.GetByCompanyAsync(companyId);
        return Ok(result);
    }
}