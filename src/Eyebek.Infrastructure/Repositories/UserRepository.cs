using Microsoft.EntityFrameworkCore;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public Task<List<User>> GetByCompanyAsync(int companyId)
        => _db.Users.Where(x => x.CompanyId == companyId).ToListAsync();

    public Task<int> CountByCompanyAsync(int companyId)
        => _db.Users.CountAsync(x => x.CompanyId == companyId);
}