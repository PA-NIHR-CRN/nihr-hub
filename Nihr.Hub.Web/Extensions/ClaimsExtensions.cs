using System.Security.Claims;

namespace Nihr.Hub.Web.Extensions;

public static class ClaimsExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(ClaimTypes.Email)?.Value.Trim();
    }

    public static string? GetGivenName(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(ClaimTypes.GivenName)?.Value.Trim();
    }

    public static string? GetFullName(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(ClaimTypes.Name)?.Value.Trim();
    }
}