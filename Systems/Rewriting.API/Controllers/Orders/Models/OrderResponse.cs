using AutoMapper;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.API.Controllers.Orders;

public class OrderResponse
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
}

public class OrderResponseProfile : Profile
{
    public OrderResponseProfile()
    {
        CreateMap<OrderModel, OrderResponse>();
    }
}
