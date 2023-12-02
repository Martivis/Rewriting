using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ResultCompareProfile : Profile
{
    public ResultCompareProfile()
    {
        CreateMap<Result, ResultCompareModel>()
            .ForMember(t => t.SourceText, o => o.MapFrom(s => s.Contract.Order.Text))
            .ForMember(t => t.ResultText, o => o.MapFrom(s => s.Content))
            .ForMember(t => t.ResultUid, o => o.MapFrom(s => s.Uid));
    }
}