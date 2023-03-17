using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ContractProfile : Profile
{
	public ContractProfile()
	{
		CreateMap<Offer, Contract>()
			.ForMember(d => d.Uid, o => o.MapFrom(s => s.OrderUid))
			.ForMember(d => d.Result, o => o.Ignore());
	}
}
