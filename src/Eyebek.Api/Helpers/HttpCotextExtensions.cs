using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Eyebek.Api.Helpers;

public static class HttpContextExtensions
{
    public static int? GetCompanyId(this HttpContext context)
    {
        var claim = context.User.FindFirst("companyId");
        if (claim == null)
            return null;

        if (int.TryParse(claim.Value, out var id))
            return id;

        return null;
    }
}