using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public virtual UserData? UserData { get; set; }
}
