using Eyebek.Application.DTOs.Attendance;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;

namespace Eyebek.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly ICompanyRepository _companies;
    private readonly IUserRepository _users;
    private readonly IAttendanceRepository _attendance;

    public AttendanceService(ICompanyRepository companies, IUserRepository users, IAttendanceRepository attendance)
    {
        _companies = companies;
        _users = users;
        _attendance = attendance;
    }

    public async Task RegisterAsync(int companyId, AttendanceCreateRequest request)
    {
        var company = await _companies.GetByIdAsync(companyId)
                      ?? throw new Exception("Empresa no encontrada.");

        if (company.Status != CompanyStatus.Activo)
            throw new Exception("Empresa sin plan activo.");

        var companyUsers = await _users.GetByCompanyAsync(companyId);
        var user = companyUsers.FirstOrDefault(x => x.Id == request.UserId)
                   ?? throw new Exception("Usuario no pertenece a la empresa.");

        if (user.Status != UserStatus.Activo)
            throw new Exception("Usuario inactivo.");

        var attendance = new Attendance
        {
            UserId = user.Id,
            Type = request.Type,
            Method = request.Method,
            Confidence = request.Confidence,
            CapturePhoto = request.CapturePhoto,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Timestamp = DateTime.UtcNow
        };

        await _attendance.AddAsync(attendance);
    }
}