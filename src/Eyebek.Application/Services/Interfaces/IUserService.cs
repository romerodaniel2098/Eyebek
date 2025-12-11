using Eyebek.Application.DTOs.Users;

namespace Eyebek.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserListItemDto> CreateAsync(int companyId, UserCreateRequest request);
    Task<List<UserListItemDto>> GetByCompanyAsync(int companyId);
}