using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Eyebek.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly AppDbContext _db;
    public AttendanceRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Attendance attendance)
    {
        _db.Attendances.Add(attendance);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Attendance>> GetByCompanyAsync(int companyId)
    {
        return await _db.Attendances
            .Include(a => a.User)
            .Where(a => a.User.CompanyId == companyId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}
