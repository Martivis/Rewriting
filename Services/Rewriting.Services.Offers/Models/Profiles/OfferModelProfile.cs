using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Offers;

public class OfferModelProfile : Profile
{
    public OfferModelProfile()
    {
        CreateMap<Offer, OfferModel>();
    }
}
