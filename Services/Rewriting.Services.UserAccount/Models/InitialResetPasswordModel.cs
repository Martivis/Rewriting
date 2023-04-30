using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class InitialResetPasswordModel
{
    public string Email { get; set; }
}
