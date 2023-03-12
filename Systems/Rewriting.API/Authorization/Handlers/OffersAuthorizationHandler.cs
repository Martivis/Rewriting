using Microsoft.AspNetCore.Authorization;
using Rewriting.Common.Helpers;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;

namespace Rewriting.API.Authorization;

public class OffersAuthorizationHandler : AuthorizationHandler<OfferClientRequirement, OfferAuthorizationModel>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OfferClientRequirement requirement,
        OfferAuthorizationModel resource)
    {
        var userUid = context.User.GetUid();
        if (userUid == resource.ClientUid)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
