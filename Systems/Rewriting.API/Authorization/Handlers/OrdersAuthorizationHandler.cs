using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Helpers;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders;
using System.Security.Claims;

namespace Rewriting.API.Authorization;

public class OrdersAuthorizationHandler : AuthorizationHandler<AuthorUidRequirement, OrderModel>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        AuthorUidRequirement requirement, 
        OrderModel resource)
    {
        var userUid = context.User.GetUid();
        if (userUid == resource.ClientUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
