using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public class OrderDetailsModelProfile : Profile
{
    public OrderDetailsModelProfile()
    {
        CreateMap<Order, OrderDetailsModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ForMember(t => t.ContractorUid, o => o.MapFrom(s => MapContractorGuid(s)))
            .ForMember(t => t.ContractorName, o => o.MapFrom(s => MapContractorName(s)))
            .ForMember(t => t.ContractPublishDate, o => o.MapFrom(s => MapContractPublishDate(s)));
    }

    private Guid? MapContractorGuid(Order source) => source.Contract?.ContractorUid;

    private string? MapContractorName(Order source)
    {
        var contractor = source.Contract?.Contractor;
        if (contractor is null)
            return null;
        return $"{contractor.FirstName} {contractor.LastName}";
    }

    private DateTime? MapContractPublishDate(Order source) => source.Contract?.PublishDate;
}
