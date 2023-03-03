using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Common.Helpers;

public static class ClaimsPrincipalHelper
{
    public static Guid GetUid(this ClaimsPrincipal principal)
    {
        var nameIdentifierClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Claim containing Uid now found");

        return Guid.Parse(nameIdentifierClaim.Value);
    }
}
