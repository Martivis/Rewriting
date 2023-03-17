using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Offers;

public class OfferAuthorizationModelProfile : Profile
{
    public OfferAuthorizationModelProfile()
    {
        CreateMap<Offer, OfferAuthorizationModel>()
            .ForMember(t => t.ClientUid, o => o.MapFrom(s => s.Order.ClientUid));
    }
}
