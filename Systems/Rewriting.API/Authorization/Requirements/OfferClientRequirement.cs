using Microsoft.AspNetCore.Authorization;

namespace Rewriting.API.Authorization;

public class OfferClientRequirement : IAuthorizationRequirement
{
    public Guid OfferClientUid { get; }
}
