using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class OrderModel
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
}

public class OrderModelProfile : Profile
{
    public OrderModelProfile()
    {
        CreateMap<Order, OrderModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ReverseMap();
    }
}
