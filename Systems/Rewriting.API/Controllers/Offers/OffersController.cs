using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rewriting.API.Controllers.Orders;
using Rewriting.Common.Helpers;
using Rewriting.Common.Security;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;

namespace Rewriting.API.Controllers.Offers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOfferService _offerService;

        public OffersController(
            IMapper mapper,
            IOfferService offerService)
        {
            _mapper = mapper;
            _offerService = offerService;
        }

        [HttpGet]
        [Authorize(Policy = AppScopes.OffersRead)]
        public async Task<IEnumerable<OfferResponse>> GetOffersByOrder(Guid orderUid)
        {
            var offers = await _offerService.GetOffersByOrder(orderUid);
            return _mapper.Map<IEnumerable<OfferResponse>>(offers);
        }

        [HttpGet]
        [Authorize(Policy = AppScopes.OffersRead)]
        public async Task<IEnumerable<OfferResponse>> GetOffersByUser()
        {
            var userUid = User.GetUid();
            var offers = await _offerService.GetOffersByUser(userUid);
            return _mapper.Map<IEnumerable<OfferResponse>>(offers);
        }

        [HttpPost]
        [Authorize(Policy = AppScopes.OffersWrite)]
        public async Task<OfferResponse> AddOffer(AddOfferRequest request)
        {
            var addOfferModel = _mapper.Map<AddOfferModel>(request);
            addOfferModel.ContractorUid = User.GetUid();
            
            var result = await _offerService.AddOffer(addOfferModel);
            return _mapper.Map<OfferResponse>(result);
        }
    }
}
