using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly AppDbContext _db;
    public AuditRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Audit audit)
    {
        _db.Audits.Add(audit);
        await _db.SaveChangesAsync();
    }
}