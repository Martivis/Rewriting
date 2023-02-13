using Duende.IdentityServer.Models;

namespace Rewriting.Identity.Configuration;

public static class AppResources
{
    public static IEnumerable<ApiResource> Resourses =>
        new List<ApiResource>()
        {
            new ApiResource("api")
        };
}
