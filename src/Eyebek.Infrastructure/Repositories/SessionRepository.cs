using Microsoft.EntityFrameworkCore;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _db;
    public SessionRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Session session)
    {
        _db.Sessions.Add(session);
        await _db.SaveChangesAsync();
    }

    public async Task DeactivateCompanySessionsAsync(int companyId)
    {
        var sessions = await _db.Sessions
            .Where(s => s.CompanyId == companyId && s.Active)
            .ToListAsync();

        foreach (var s in sessions)
            s.Active = false;

        if (sessions.Count > 0)
            await _db.SaveChangesAsync();
    }
}