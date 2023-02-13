using Duende.IdentityServer.Models;

namespace Rewriting.Identity.Configuration;

public class AppIdentityResources
{
    public static IEnumerable<IdentityResource> Resources =>
        new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
}
