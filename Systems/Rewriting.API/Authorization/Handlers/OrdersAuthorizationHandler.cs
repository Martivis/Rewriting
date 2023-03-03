using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Helpers;
using Rewriting.Context.Entities;
using System.Security.Claims;

namespace Rewriting.API.Authorization;

public class OrdersAuthorizationHandler : AuthorizationHandler<AuthorUidRequirement, Order>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        AuthorUidRequirement requirement, 
        Order resource)
    {
        var userUid = context.User.GetUid();
        if (userUid == resource.ClientUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
