using Eyebek.Domain.Entities;

namespace Eyebek.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<List<User>> GetByCompanyAsync(int companyId);
    Task<int> CountByCompanyAsync(int companyId);
}