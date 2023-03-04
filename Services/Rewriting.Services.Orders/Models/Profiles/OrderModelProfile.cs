using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public class OrderModelProfile : Profile
{
    public OrderModelProfile()
    {
        CreateMap<Order, OrderModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ReverseMap();
    }
}
