using Microsoft.AspNetCore.Authorization;
using Rewriting.Common.Helpers;
using Rewriting.Services.Contracts;
using Rewriting.Services.Offers;

namespace Rewriting.API.Authorization;

public class ContractorAuthorizationHandler : AuthorizationHandler<OwnerUidRequirement, ContractorAuthModel>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerUidRequirement requirement,
        ContractorAuthModel resource)
    {
        var userUid = context.User.GetUid();
        if (userUid == resource.ContractorUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
