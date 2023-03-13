using AutoMapper;
using Rewriting.Services.Contracts;

namespace Rewriting.API.Controllers.Contracts;

public class ContractDetailsResponseProfile : Profile
{
    public ContractDetailsResponseProfile()
    {
        CreateMap<ContractDetailsModel, ContractDetailsResponse>();
    }
}
