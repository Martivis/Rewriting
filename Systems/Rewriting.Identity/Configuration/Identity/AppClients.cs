using Duende.IdentityServer.Models;
using Rewriting.Common.Security;

namespace Rewriting.Identity.Configuration;

public static class AppClients
{
    public static IEnumerable<Client> Clients =>
        new List<Client>()
        {
            new Client()
            {
                ClientId = "swagger",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AccessTokenLifetime = 60,
                AccessTokenType = AccessTokenType.Jwt,
                AllowedScopes =
                {
                    AppScopes.OrdersRead,
                    AppScopes.OrdersWrite,
                    AppScopes.OffersRead,
                    AppScopes.OffersWrite,
                },

            },

            new Client()
            {
                ClientId = "frontend",
                ClientSecrets = { new Secret("secret".Sha256()), },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AccessTokenLifetime = 60,
                AccessTokenType = AccessTokenType.Jwt,
                AllowOfflineAccess = true,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                SlidingRefreshTokenLifetime = 60,
                AbsoluteRefreshTokenLifetime = 360,
                AllowedScopes =
                {
                    AppScopes.OrdersRead,
                    AppScopes.OrdersWrite,
                    AppScopes.OffersRead,
                    AppScopes.OffersWrite,
                }
            }
        };
}
