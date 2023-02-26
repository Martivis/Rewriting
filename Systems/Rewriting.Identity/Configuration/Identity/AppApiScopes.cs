using Duende.IdentityServer.Models;
using Rewriting.Common.Security;
using System.Security.Claims;

namespace Rewriting.Identity.Configuration;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(AppScopes.OrdersRead, "Access to orders API - Read orders"),
            new ApiScope(AppScopes.OrdersWrite, "Access to orders API - Add orders"),
            new ApiScope(AppScopes.OrdersEdit, "Access to orders API - Edit orders"),
            new ApiScope(AppScopes.OffersRead, "Access to offers API - Read offers"),
            new ApiScope(AppScopes.OffersWrite, "Access to offers API - Add offers")
        };
}
