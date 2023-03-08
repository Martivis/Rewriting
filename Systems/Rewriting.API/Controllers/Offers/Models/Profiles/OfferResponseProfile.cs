using AutoMapper;
using Rewriting.Services.Offers;

namespace Rewriting.API.Controllers.Offers;

public class OfferResponseProfile : Profile
{
    public OfferResponseProfile()
    {
        CreateMap<OfferModel, OfferResponse>();
    }
}
