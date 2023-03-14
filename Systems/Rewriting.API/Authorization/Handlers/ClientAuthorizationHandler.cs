using Microsoft.AspNetCore.Authorization;
using Rewriting.Common.Helpers;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Authorization;

public class ClientAuthorizationHandler : AuthorizationHandler<OwnerUidRequirement, ClientAuthModel>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerUidRequirement requirement,
        ClientAuthModel resource)
    {
        var userUid = context.User.GetUid();
        if (userUid == resource.ClientUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
