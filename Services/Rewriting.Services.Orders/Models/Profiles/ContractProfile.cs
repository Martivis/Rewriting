using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Models;

public class ContractProfile : Profile
{
	public ContractProfile()
	{
		CreateMap<Offer, Contract>()
			.ForMember(d => d.Uid, o => o.MapFrom(s => s.OrderUid))
			.ForMember(d => d.Result, o => o.Ignore());
	}
}
