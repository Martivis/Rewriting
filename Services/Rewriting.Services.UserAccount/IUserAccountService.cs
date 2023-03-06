using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public interface IUserAccountService
{
    bool IsAnyUsers();
    Task<UserModel> Create(RegisterUserModel model);
    Task ChangePassword(ChangePasswordModel model);
    Task AddToRole(AddToRoleModel model);
}
