using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

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
}
