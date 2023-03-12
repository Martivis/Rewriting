using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public class OfferAuthorizationModelProfile : Profile
{
    public OfferAuthorizationModelProfile()
    {
        CreateMap<Offer, OfferAuthorizationModel>()
            .ForMember(t => t.ClientUid, o => o.MapFrom(s => s.Order.ClientUid));
    }
}
