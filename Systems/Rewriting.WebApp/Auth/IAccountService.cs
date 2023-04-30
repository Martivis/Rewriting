using MudBlazor;

namespace Rewriting.WebApp
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(ChangePasswordModel changePasswordModel);
        Task RegisterAsync(RegisterModel registerModel);
        Task InitialPasswordResetAsync(InitialResetPasswordModel initialResetPasswordModel);
        Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
    }
}