using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public class OrderDetailsModelProfile : Profile
{
    public OrderDetailsModelProfile()
    {
        CreateMap<Order, OrderDetailsModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"));
    }
}
