using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Offers
{
    internal class OfferService : IOfferService, IOfferObservable
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public event Action<OfferModel> OnOfferAdd;
        public event Action<OfferModel> OnOfferAccept;

        public OfferService(
            IDbContextFactory<AppDbContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<OfferAuthorizationModel> GetOfferAuthAsync(Guid offerUid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var offer = context.Set<Offer>().Find(offerUid)
                ?? throw new ProcessException($"Offer {offerUid} not found");

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

            using var context = await _contextFactory.CreateDbContextAsync();

            var offers = context.Set<Offer>()
                .Where(offer => offer.OrderUid == orderUid)
                .OrderByDescending(offer => offer.PublishDate)
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IEnumerable<OfferModel>>(offers);
        }

        public async Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");
            if (pageSize > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size is too big");

            using var context = await _contextFactory.CreateDbContextAsync();

            var offers = context.Set<Offer>()
                .Where(offer => offer.ContractorUid == userUid)
                .OrderByDescending(offer => offer.PublishDate)
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToList();

            return _mapper.Map<IEnumerable<OfferModel>>(offers);
        }

        public async Task<OfferModel> AddOfferAsync(AddOfferModel model)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var order = context.Set<Order>().Find(model.OrderUid)
                ?? throw new ProcessException($"Order {model.OrderUid} not found");

            if (order.ClientUid == model.ContractorUid)
                throw new ProcessException("Unable to add offer to your own order");
            if (order.Status != OrderStatus.New)
                throw new ProcessException("Unable to add offer");

            var offer = _mapper.Map<Offer>(model);
            offer.PublishDate = DateTime.UtcNow;

            context.Add(offer);

            context.SaveChanges();

            var offerModel = _mapper.Map<OfferModel>(offer);
            OnOfferAdd.Invoke(offerModel);

            return offerModel;
        }

        public async Task AcceptOfferAsync(Guid offerUid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var offer = context.Set<Offer>().Find(offerUid)
                ?? throw new ProcessException($"Offer {offerUid} not found");

            if (offer.Order.Status != OrderStatus.New)
                throw new ProcessException($"Unable to add contract to order {offer.OrderUid}");

            offer.Order.Status = OrderStatus.InProgress;

            var contract = _mapper.Map<Contract>(offer);
            contract.PublishDate = DateTime.UtcNow;

            context.Add(contract);
            context.SaveChanges();

            var offerMovel = _mapper.Map<OfferModel>(offer);
            OnOfferAccept.Invoke(offerMovel);
        }
    }
}
