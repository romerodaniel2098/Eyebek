using System.Security.Claims;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;

namespace Eyebek.Api.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuditRepository audits)
    {
        await _next(context);
        
        var companyIdClaim = context.User.FindFirst("companyId");
        if (companyIdClaim is null) return;

        var companyId = int.Parse(companyIdClaim.Value);

        var audit = new Audit
        {
            CompanyId = companyId,
            Action = context.Request.Method,
            Entity = context.Request.Path.Value ?? "unknown",
            Description = "Request audit",
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers.UserAgent.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await audits.AddAsync(audit);
        }
        catch
    }
}