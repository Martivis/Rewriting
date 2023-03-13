using AutoMapper;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Controllers.Contracts;

public class ContractResponseProfile : Profile
{
    public ContractResponseProfile()
    {
        CreateMap<ContractModel, ContractResponse>();
    }
}
