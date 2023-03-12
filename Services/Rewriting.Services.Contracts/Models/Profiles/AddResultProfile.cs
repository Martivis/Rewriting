using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
