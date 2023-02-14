using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public class UserModel
{
    public Guid Uid { get; set; }
    public string Email { get; set; }
}

public class UserAccountProfile : Profile
{
    public UserAccountProfile()
    {
        CreateMap<UserIdentity, UserModel>()
            .ForMember(d => d.Uid, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
    }
}
