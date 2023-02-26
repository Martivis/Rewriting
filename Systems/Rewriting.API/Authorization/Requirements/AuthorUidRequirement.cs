using Microsoft.AspNetCore.Authorization;

namespace Rewriting.API.Authorization;

public class AuthorUidRequirement : IAuthorizationRequirement
{
    public Guid UserUid { get; } 
}
