using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eyebek.Application.DTOs.Users;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;

namespace Eyebek.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    // Crea un usuario para una empresa
    public async Task<UserListItemDto> CreateAsync(int companyId, UserCreateRequest request)
    {
        var user = new User
        {
            CompanyId   = companyId,
            Name        = request.Name,
            Email       = request.Email,
            Document    = request.Document,
            Role        = request.Role,
            Status      = UserStatus.Active,
            Phone       = request.Phone,
            Photo       = request.Photo,
            FacialEmbedding = request.FacialEmbedding,
            PasswordHash = string.Empty,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };

        await _users.AddAsync(user);

        return Map(user);
    }

    // Lista de usuarios por empresa
    public async Task<List<UserListItemDto>> GetByCompanyAsync(int companyId)
    {
        var users = await _users.GetByCompanyAsync(companyId);
        return users
            .Select(Map)
            .ToList();
    }

    private static UserListItemDto Map(User u) => new()
    {
        Id     = u.Id,
        Name   = u.Name,
        Email  = u.Email,
        Role   = u.Role,
        Status = u.Status,
        Document = u.Document,
        Phone = u.Phone,
        Photo = u.Photo
    };
}