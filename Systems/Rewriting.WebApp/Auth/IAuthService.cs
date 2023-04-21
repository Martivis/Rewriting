namespace Rewriting.WebApp;

public interface IAuthService
{
    Task<string> GetAccessTokenAsync();
    Task<LoginResult> LoginAsync(LoginModel loginModel);
    Task LogoutAsync();
}
