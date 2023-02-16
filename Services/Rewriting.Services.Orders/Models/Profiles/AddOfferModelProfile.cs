using AutoMapper;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public class AddOfferModelProfile : Profile
{
    public AddOfferModelProfile()
    {
        CreateMap<AddOfferModel, Offer>();
    }
}
