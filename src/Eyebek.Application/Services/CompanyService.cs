using Eyebek.Application.DTOs.Companies;
using Eyebek.Application.Interfaces;
using Eyebek.Application.Services.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Domain.Enums;

namespace Eyebek.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companies;
    private readonly ISessionRepository _sessions;
    private readonly IJwtTokenGenerator _jwt;

    public CompanyService(
        ICompanyRepository companies,
        ISessionRepository sessions,
        IJwtTokenGenerator jwt)
    {
        _companies = companies;
        _sessions = sessions;
        _jwt = jwt;
    }

    public async Task<CompanyMeResponse> RegisterAsync(CompanyRegisterRequest request)
    {
        // Validar si ya existe una empresa con ese email
        var existing = await _companies.GetByEmailAsync(request.Email);
        if (existing != null)
            throw new Exception("El email ya está registrado.");

        var company = new Company
        {
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            // Hasheamos la contraseña con BCrypt
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Status = CompanyStatus.SinPlan,
            CurrentUsers = 0
        };

        await _companies.AddAsync(company);

        return MapMe(company);
    }

    public async Task<(string token, CompanyMeResponse company)> LoginAsync(
        CompanyLoginRequest request,
        string? ip,
        string? userAgent)
    {
        var company = await _companies.GetByEmailAsync(request.Email)
            ?? throw new Exception("Credenciales inválidas.");

        // Verificar contraseña
        var ok = BCrypt.Net.BCrypt.Verify(request.Password, company.PasswordHash);
        if (!ok)
            throw new Exception("Credenciales inválidas.");

        // Expira plan automáticamente si ya pasó la fecha
        if (company.PlanEndDate.HasValue && company.PlanEndDate.Value < DateTime.UtcNow)
        {
            company.Status = CompanyStatus.Vencido;
            await _companies.UpdateAsync(company);
        }

        // Desactivar sesiones previas para mantener solo una activa (opcional)
        await _sessions.DeactivateCompanySessionsAsync(company.Id);

        // Generar token JWT
        var token = _jwt.GenerateCompanyToken(company);

        // Registrar sesión
        var session = new Session
        {
            CompanyId = company.Id,
            Token = token,
            IpAddress = ip,
            UserAgent = userAgent,
            StartDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(120),
            Active = true
        };

        await _sessions.AddAsync(session);

        return (token, MapMe(company));
    }

    public async Task<CompanyMeResponse> GetMeAsync(int companyId)
    {
        var company = await _companies.GetByIdAsync(companyId)
            ?? throw new Exception("Empresa no encontrada.");

        return MapMe(company);
    }

    public async Task<CompanyMeResponse> UpdateMeAsync(int companyId, CompanyUpdateRequest request)
    {
        var company = await _companies.GetByIdAsync(companyId)
            ?? throw new Exception("Empresa no encontrada.");

        company.Name = request.Name;
        company.Phone = request.Phone;
        company.Address = request.Address;

        await _companies.UpdateAsync(company);

        return MapMe(company);
    }

    private static CompanyMeResponse MapMe(Company c) => new CompanyMeResponse
    {
        Id = c.Id,
        Name = c.Name,
        Email = c.Email,
        Status = c.Status,
        PlanId = c.PlanId,
        PlanStartDate = c.PlanStartDate,
        PlanEndDate = c.PlanEndDate,
        CurrentUsers = c.CurrentUsers
    };
}
