using Eyebek.Domain.Entities;

namespace Eyebek.Application.Interfaces;

public interface IAttendanceRepository
{
    Task AddAsync(Attendance attendance);
    Task<List<Attendance>> GetByCompanyAsync(int companyId);
}