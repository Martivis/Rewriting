using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class ResetPasswordModel
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
