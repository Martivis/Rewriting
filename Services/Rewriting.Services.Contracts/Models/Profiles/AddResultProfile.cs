using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class AddResultProfile : Profile
{
    public AddResultProfile()
    {
        CreateMap<AddResultModel, Result>()
            .ForMember(t => t.Status, o => o.Ignore())
            .ForMember(t => t.Content, o => o.MapFrom(s => s.Text));
    }
}
