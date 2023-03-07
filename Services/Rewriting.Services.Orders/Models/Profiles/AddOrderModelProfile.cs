using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public class AddOrderModelProfile : Profile
{
    public AddOrderModelProfile()
    {
        CreateMap<AddOrderModel, Order>()
            .ForMember(t => t.DateTime, o => o.Ignore())
            .ForMember(t => t.Status, o => o.Ignore());
    } // TODO: Add tests
}