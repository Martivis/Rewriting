using AutoMapper;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts;

public class UserAccountResponse
{
    public Guid Uid { get; set; }
    public string Email { get; set; }
}

public class UserAccountResponseProfile : Profile
{
    public UserAccountResponseProfile()
    {
        CreateMap<UserModel, UserAccountResponse>();
    }
}
