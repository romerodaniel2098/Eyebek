using System.Security.Claims;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
        // Dejamos pasar la petición primero
        await _next(context);

        try
        {
            // Sacar companyId del token, si existe
            var companyIdClaim = context.User.FindFirst("companyId");
            if (companyIdClaim == null)
                return;

            if (!int.TryParse(companyIdClaim.Value, out var companyId))
                return;

            var ip = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var path = context.Request.Path.ToString();
            var method = context.Request.Method;

            var audit = new Audit
            {
                CompanyId = companyId,
                Action = method,
                Entity = path,
                IpAddress = ip,
                UserAgent = userAgent,
                CreatedAt = DateTime.UtcNow
            };

            await audits.AddAsync(audit);
        }
        catch
        {
            // No rompemos la petición si falla la auditoría
        }
    }
}

public static class AuditMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuditMiddleware>();
    }
}