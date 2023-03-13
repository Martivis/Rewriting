using Microsoft.AspNetCore.Authorization;
using Rewriting.Common.Helpers;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Authorization.Handlers
{
    public class ClientAuthorizationHandler : AuthorizationHandler<OwnerUidRequirement, ContractModel>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnerUidRequirement requirement,
            ContractModel resource)
        {
            var userUid = context.User.GetUid();
            if (userUid == resource.ClientUid)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
