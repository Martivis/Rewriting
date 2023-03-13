using Microsoft.AspNetCore.Authorization;

namespace Rewriting.API.Authorization;

public class OwnerUidRequirement : IAuthorizationRequirement
{
    public Guid UserUid { get; } 
}
