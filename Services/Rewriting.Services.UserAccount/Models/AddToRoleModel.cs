using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class AddToRoleModel
{
    public Guid UserUid { get; set; }
    public string RoleName { get; set; }
}
