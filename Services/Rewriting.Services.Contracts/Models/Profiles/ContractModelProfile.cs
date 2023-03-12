using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ContractModelProfile : Profile
{
    public ContractModelProfile()
    {
        CreateMap<Order, ContractModel>()
            .ForMember(t => t.ClientName, o => o.MapFrom(s => $"{s.Client.FirstName} {s.Client.LastName}"))
            .ForMember(t => t.Price, o =>  o.MapFrom(s => s.Contract!.Price))
            .ReverseMap();
    }
}
