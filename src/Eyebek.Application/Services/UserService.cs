using Eyebek.Application.DTOs.Users;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;

namespace Eyebek.Application.Services;

public class UserService : IUserService
{
    private readonly ICompanyRepository _companies;
    private readonly IUserRepository _users;

    public UserService(ICompanyRepository companies, IUserRepository users)
    {
        _companies = companies;
        _users = users;
    }

    public async Task<UserListItemDto> CreateAsync(int companyId, UserCreateRequest request)
    {
        var company = await _companies.GetByIdAsync(companyId)
            ?? throw new Exception("Empresa no encontrada.");

        if (company.Status != CompanyStatus.Activo || company.PlanId is null || company.Plan is null)
            throw new Exception("La empresa no tiene un plan activo.");

        var currentCount = await _users.CountByCompanyAsync(companyId);

        if (currentCount >= company.Plan.UserCapacity)
            throw new Exception("Límite de usuarios del plan alcanzado.");

        var user = new User
        {
            CompanyId = companyId,
            Name = request.Name,
            Document = request.Document,
            Email = request.Email,
            Phone = request.Phone,
            Role = request.Role,
            Status = UserStatus.Active
        };

        await _users.AddAsync(user);

        company.CurrentUsers = currentCount + 1;
        await _companies.UpdateAsync(company);

        return new UserListItemDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status
        };
    }

    public async Task<List<UserListItemDto>> GetByCompanyAsync(int companyId)
    {
        var list = await _users.GetByCompanyAsync(companyId);

        return list.Select(u => new UserListItemDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            Status = u.Status
        }).ToList();
    }
}
