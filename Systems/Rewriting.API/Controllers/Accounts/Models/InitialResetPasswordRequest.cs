using AutoMapper;
using Rewriting.Services.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.API.Controllers.Accounts;

public class InitialResetPasswordRequest
{
    public string Email { get; set; }
}

public class InitialResetPasswordRequestProfile : Profile
{
    public InitialResetPasswordRequestProfile()
    {
        CreateMap<InitialResetPasswordRequest, InitialResetPasswordModel>();
    }
}
