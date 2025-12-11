using System.Security.Claims;

namespace Eyebek.Api.Helpers;

public static class HttpContextExtensions
{
    public static int GetCompanyId(this HttpContext httpContext)
    {
        var value = httpContext.User.FindFirstValue("companyId");
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Token sin companyId.");
        return int.Parse(value);
    }
}