using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Exceptions;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

internal class UserDataService : IUserDataService
{
    private UserManager<ApplicationUser> _userManager;

    public UserDataService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> GetUserEmailAsync(Guid userUid)
    {
        var orderOwner = await _userManager.FindByIdAsync(userUid.ToString())
            ?? throw new ProcessException($"User {userUid} not found");

        if (orderOwner.Email is null)
            throw new ProcessException($"User {userUid} doesn't have an email");

        return orderOwner.Email;
    }
}
