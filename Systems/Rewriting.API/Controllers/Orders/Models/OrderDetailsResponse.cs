using AutoMapper;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.API.Controllers.Orders;

public class OrderDetailsResponse
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
    public DateTime PublishDate { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
}

public class OrderDetailsResponseProfile : Profile
{
    public OrderDetailsResponseProfile()
    {
        CreateMap<OrderDetailsModel, OrderDetailsResponse>();
    }
}