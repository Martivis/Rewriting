namespace Rewriting.WebApp
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(ChangePasswordModel changePasswordModel);
        Task RegisterAsync(RegisterModel registerModel);
    }
}