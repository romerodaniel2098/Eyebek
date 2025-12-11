using Eyebek.Application.DTOs.Companies;

namespace Eyebek.Application.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyMeResponse> RegisterAsync(CompanyRegisterRequest request);
    Task<(string token, CompanyMeResponse company)> LoginAsync(CompanyLoginRequest request, string? ip, string? userAgent);
    Task<CompanyMeResponse> GetMeAsync(int companyId);
    Task<CompanyMeResponse> UpdateMeAsync(int companyId, CompanyUpdateRequest request);
}