using Eyebek.Application.DTOs.Attendance;

namespace Eyebek.Application.Services.Interfaces;

public interface IAttendanceService
{
    Task RegisterAsync(int companyId, AttendanceCreateRequest request);
}