using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class OrderDetailsModelProfile : Profile
{
    public OrderDetailsModelProfile()
    {
        CreateMap<Order, OrderDetailsModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ForMember(t => t.Result, o => o.MapFrom(s => MapResult(s)))
            .ForMember(t => t.ContractorUid, o => o.MapFrom(s => MapContractorGuid(s)))
            .ForMember(t => t.ContractorName, o => o.MapFrom(s => MapContractorName(s)));
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
