using Eyebek.Application.DTOs.Attendance;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Domain.Entities;

namespace Eyebek.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }
    
    public async Task RegisterAsync(int companyId, AttendanceCreateRequest request)
    {
        var attendance = new Attendance
        {
            UserId = request.UserId,
            Type = request.Type,
            Method = request.Method,
            Confidence = request.Confidence,
            CapturePhoto = request.CapturePhoto,
            CreatedAt = DateTime.UtcNow
        };

        await _attendanceRepository.AddAsync(attendance);
    }
}