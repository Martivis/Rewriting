using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ResultModelProfile : Profile
{
    public ResultModelProfile()
    {
        CreateMap<Result, ResultModel>()
            .ForMember(t => t.Text, o => o.MapFrom(s => s.Content));
    }
}
