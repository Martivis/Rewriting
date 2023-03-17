using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ContractorAuthModelProfile : Profile
{
    public ContractorAuthModelProfile()
    {
        CreateMap<Contract, ContractorAuthModel>();
    }
}
