using Microsoft.EntityFrameworkCore;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _db;
    public CompanyRepository(AppDbContext db) => _db = db;

    public Task<Company?> GetByEmailAsync(string email)
        => _db.Companies.Include(x => x.Plan).FirstOrDefaultAsync(x => x.Email == email);

    public Task<Company?> GetByIdAsync(int id)
        => _db.Companies.Include(x => x.Plan).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Company company)
    {
        _db.Companies.Add(company);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        company.UpdatedAt = DateTime.UtcNow;
        _db.Companies.Update(company);
        await _db.SaveChangesAsync();
    }
}