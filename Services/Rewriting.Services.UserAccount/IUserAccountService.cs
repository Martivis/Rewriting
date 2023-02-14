using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public interface IUserAccountService
{
    Task<UserModel> Create(RegisterUserModel model);
    Task ChangePassword(ChangePasswordModel model);
}
