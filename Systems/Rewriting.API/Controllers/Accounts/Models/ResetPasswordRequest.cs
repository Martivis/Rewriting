using AutoMapper;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts;

public class ResetPasswordRequest
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}

public class ResetPasswordRequestProfile : Profile
{
    public ResetPasswordRequestProfile()
    {
        CreateMap<ResetPasswordRequest, ResetPasswordModel>();
    }
}