using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ClientAuthModelProfile : Profile
{
    public ClientAuthModelProfile()
    {
        CreateMap<Contract, ClientAuthModel>()
            .ForMember(t => t.ClientUid, o => o.MapFrom(s => s.Order.ClientUid));
    }
}
