using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ContractDetailsModelProfile : Profile
{
    public ContractDetailsModelProfile()
    {
        CreateMap<Order, ContractDetailsModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ForMember(t => t.Results, o => o.MapFrom(s => MapResult(s)))
            .ForMember(t => t.ContractorUid, o => o.MapFrom(s => MapContractorGuid(s)))
            .ForMember(t => t.ContractorName, o => o.MapFrom(s => MapContractorName(s)))
            .ForMember(t => t.Price, o => o.MapFrom(s => s.Contract!.Price));
    }

    private IEnumerable<string> MapResult(Order source) => source.Contract?.Result.Select(result => result.Content)
        ?? new List<string>();

    private Guid? MapContractorGuid(Order source) => source.Contract?.ContractorUid;

    private string? MapContractorName(Order source)
    {
        var contractor = source.Contract?.Contractor;
        if (contractor is null)
            return null;
        return $"{contractor.FirstName} {contractor.LastName}";
    }
}
