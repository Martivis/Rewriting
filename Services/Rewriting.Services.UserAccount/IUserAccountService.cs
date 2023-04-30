using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Exceptions;
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
    Task<UserModel> CreateAsync(RegisterUserModel model);
    Task ChangePasswordAsync(ChangePasswordModel model);
    Task AddToRoleAsync(AddToRoleModel model);
    Task InitializePasswordReset(InitialResetPasswordModel model);
    Task ResetPassword(ResetPasswordModel model);
}
