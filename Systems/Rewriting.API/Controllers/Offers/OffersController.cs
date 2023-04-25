using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Helpers;
using Rewriting.Common.Security;
using Rewriting.Services.Offers;

namespace Rewriting.API.Controllers.Offers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOfferService _offerService;
        private readonly IAuthorizationService _authorizationService;

        public OffersController(
            IMapper mapper,
            IOfferService offerService,
            IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _offerService = offerService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get offers relating to the order with specified Uid
        /// </summary>
        /// <param name="orderUid">Uid of the target order</param>
        /// <param name="page">Page number (starting with 0)</param>
        /// <param name="pageSize">Offers number per page</param>
        /// <returns>IEnumerable of OfferResponse</returns>
        [HttpGet]
        [Authorize(Policy = AppScopes.OffersRead)]
        public async Task<IEnumerable<OfferResponse>> GetOffersByOrder([FromQuery] Guid orderUid, 
                                                                       [FromQuery] int page = 0, 
                                                                       [FromQuery] int pageSize = 10)
        {
            var offers = await _offerService.GetOffersByOrderAsync(orderUid, page, pageSize);
            return _mapper.Map<IEnumerable<OfferResponse>>(offers);
        }

        /// <summary>
        /// Get offers placed by calling user
        /// </summary>
        /// <param name="page">Page number (starting with 0)</param>
        /// <param name="pageSize">Offers number per page</param>
        /// <returns>IEnumerable of OfferResponse</returns>
        [HttpGet]
        [Authorize(Policy = AppScopes.OffersRead)]
        public async Task<IEnumerable<OfferResponse>> GetOffersByUser([FromQuery] int page = 0,
                                                                      [FromQuery] int pageSize = 10)
        {
            var userUid = User.GetUid();
            var offers = await _offerService.GetOffersByUserAsync(userUid, page, pageSize);
            return _mapper.Map<IEnumerable<OfferResponse>>(offers);
        }

        /// <summary>
        /// Add new offer
        /// </summary>
        /// <param name="request">AddOfferRequest</param>
        /// <returns>OfferResponse with information about created offer</returns>
        [HttpPost]
        [Authorize(Policy = AppScopes.OffersWrite)]
        public async Task<OfferResponse> AddOffer([FromBody] AddOfferRequest request)
        {
            var addOfferModel = _mapper.Map<AddOfferModel>(request);
            addOfferModel.ContractorUid = User.GetUid();
            
            var result = await _offerService.AddOfferAsync(addOfferModel);
            return _mapper.Map<OfferResponse>(result);
        }

        /// <summary>
        /// Accept specified offer
        /// </summary>
        /// <param name="request">Uid of the target offer</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AcceptOffer([FromBody] OfferUidRequest request)
        {
            var offer = await _offerService.GetOfferAuthAsync(request.OfferUid);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, offer, AppScopes.OffersEdit);
            if (!authorizationResult.Succeeded)
                return Forbid();

            await _offerService.AcceptOfferAsync(request.OfferUid);
            return Ok();
        }
    }
}
