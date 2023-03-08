using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Common.Validator;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers
{
    internal class OfferService : IOfferService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public OfferService(
            IDbContextFactory<AppDbContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferModel>> GetOffersByOrder(Guid orderUid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var offers = context.Set<Offer>()
                .Where(offer => offer.OrderUid == orderUid)
                .ToList();

            return _mapper.Map<IEnumerable<OfferModel>>(offers);
        }

        public async Task<IEnumerable<OfferModel>> GetOffersByUser(Guid userUid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var offers = context.Set<Offer>()
                .Where(offer => offer.ContractorUid == userUid)
                .ToList();

            return _mapper.Map<IEnumerable<OfferModel>>(offers);
        }

        public async Task<OfferModel> AddOffer(AddOfferModel model)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var order = await context.Set<Order>().FindAsync(model.OrderUid)
                ?? throw new ProcessException($"Order {model.OrderUid} not found");

            if (order.ClientUid == model.ContractorUid)
                throw new ProcessException("Unable to add offer to your own order");
            if (order.Status != OrderStatus.New)
                throw new ProcessException("Unable to add offer");

            var offer = _mapper.Map<Offer>(model);
            await context.AddAsync(offer);

            context.SaveChanges();

            return _mapper.Map<OfferModel>(offer);
        }
    }
}
