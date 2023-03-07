using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Offers;

public class AddOfferModelProfile : Profile
{
    public AddOfferModelProfile()
    {
        CreateMap<AddOfferModel, Offer>();
    }
}
