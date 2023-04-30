using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;
using Rewriting.Services.Contracts;
using Rewriting.Services.Orders;

namespace Rewriting.Services.Offers
{
    internal class OfferService : IOfferService, IOfferObservable
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;

        public event Action<OfferModel> OnOfferAdd;
        public event Action<OfferModel> OnOfferAccept;

        public OfferService(IOrderRepository orderRepository, 
            IOfferRepository offerRepository, 
            IContractRepository contractRepository, 
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _offerRepository = offerRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<OfferAuthorizationModel> GetOfferAuthAsync(Guid offerUid)
        {
            var offer = await _offerRepository.GetOfferAsync(offerUid);

            return _mapper.Map<OfferAuthorizationModel>(offer);
        }

        public async Task<IEnumerable<OfferModel>> GetOffersByOrderAsync(Guid orderUid, int page = 0, int pageSize = 10)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");
            if (pageSize > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size is too big");

            return await _offerRepository.GetOffersByOrderAsync(orderUid, page, pageSize);
        }

        public async Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");
            if (pageSize > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size is too big");

            return await _offerRepository.GetOffersByUserAsync(userUid, page, pageSize);
        }

        public async Task<OfferModel> AddOfferAsync(AddOfferModel model)
        {
            var order = await _orderRepository.GetOrderAsync(model.OrderUid);

            if (order.ClientUid == model.ContractorUid)
                throw new ProcessException("Unable to add offer to your own order");
            if (order.Status != OrderStatus.New)
                throw new ProcessException("Unable to add offer");

            var offer = _mapper.Map<Offer>(model);
            offer.PublishDate = DateTime.UtcNow;

            await _offerRepository.AddOfferAsync(offer);

            var offerModel = _mapper.Map<OfferModel>(offer);
            OnOfferAdd.Invoke(offerModel);

            return offerModel;
        }

        public async Task AcceptOfferAsync(Guid offerUid)
        {
            var offer = await _offerRepository.GetOfferAsync(offerUid);

            if (offer.Order.Status != OrderStatus.New)
                throw new ProcessException($"Unable to add contract to order {offer.OrderUid}");

            offer.Order.Status = OrderStatus.InProgress;

            var contract = _mapper.Map<Contract>(offer);
            contract.PublishDate = DateTime.UtcNow;

            await _contractRepository.AddContractAsync(contract);

            var offerModel = _mapper.Map<OfferModel>(offer);
            OnOfferAccept.Invoke(offerModel);
        }
    }
}
