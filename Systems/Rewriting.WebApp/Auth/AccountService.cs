﻿namespace Rewriting.WebApp;

public class AccountService : IAccountService
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;

    public AccountService(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
    }

    public async Task RegisterAsync(RegisterModel registerModel)
    {
        await _apiService.PostDataAsync("UserAccount", registerModel);

        var loginModel = new LoginModel()
        {
            Email = registerModel.Email,
            Password = registerModel.Password,
        };

        await _authService.LoginAsync(loginModel);
    }

    public async Task ChangePasswordAsync(ChangePasswordModel changePasswordModel)
    {
        await _apiService.PatchDataAsync("UserAccount", changePasswordModel);
    }
}
