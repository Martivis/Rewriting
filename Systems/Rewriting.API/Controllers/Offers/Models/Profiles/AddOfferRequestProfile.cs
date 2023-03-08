using AutoMapper;
using Rewriting.Services.Offers;

namespace Rewriting.API.Controllers.Offers;

public class AddOfferRequestProfile : Profile
{
    public AddOfferRequestProfile()
    {
        CreateMap<AddOfferRequest, AddOfferModel>()
            .ForMember(t => t.ContractorUid, o => o.Ignore());
    }
}
