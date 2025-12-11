using Microsoft.EntityFrameworkCore;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _db;
    public PlanRepository(AppDbContext db) => _db = db;

    public Task<List<Plan>> GetActiveAsync()
        => _db.Plans.Where(x => x.Active).ToListAsync();

    public Task<Plan?> GetByIdAsync(int id)
        => _db.Plans.FirstOrDefaultAsync(x => x.Id == id);
}