using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rewriting.Context.Entities;
using System.Security.Claims;

namespace Rewriting.API.Authorization;

public class OrdersAuthorizationHandler : AuthorizationHandler<AuthorUidRequirement, Order>
{
    private UserManager<UserIdentity> _userManager;
    public OrdersAuthorizationHandler(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        AuthorUidRequirement requirement, 
        Order resource)
    {
        var userUid = GetUserUid(context.User);
        if (userUid == resource.ClientUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }

    private Guid GetUserUid(ClaimsPrincipal user)
    {
        var uid = _userManager.GetUserId(user)
            ?? throw new InvalidOperationException("User uid not found");
        return Guid.Parse(uid);
    }
}
