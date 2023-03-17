using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rewriting.Common.Security;
using Rewriting.Context.Entities;
using Rewriting.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public static class AppDbSeeder
{
    const string AdminEmail = "admin@mail.ru";
    const string AdminPassword = "Password123$"; // TODO: Hide secrets

    public static async void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();

        await AddAdmin(scope);
    }

    private static async Task AddRoles(IServiceScope scope)
    {
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>()!;

        var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>(AppRoles.Admin)
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    private static async Task AddAdmin(IServiceScope scope)
    {

        var userAccountService = scope.ServiceProvider.GetService<IUserAccountService>()
            ?? throw new InvalidOperationException($"Unable to get {nameof(IUserAccountService)}");

        if (userAccountService.IsAnyUsers())
            return;

        var admin = new RegisterUserModel
        {
            Email = AdminEmail,
            Password = AdminPassword,
            FirstName = "Admin",
            LastName = "Palych"
        };
        var registered = await userAccountService.CreateAsync(admin);

        var addToRoleModel = new AddToRoleModel
        {
            UserUid = registered.Uid,
            RoleName = AppRoles.Admin,
        };

        await userAccountService.AddToRoleAsync(addToRoleModel);
    }
}
