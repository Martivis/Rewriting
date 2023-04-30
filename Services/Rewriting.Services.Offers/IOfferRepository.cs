using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public interface IOfferRepository
{
    Task<Offer> GetOfferAsync(Guid offerUid);
    Task<IEnumerable<OfferModel>> GetOffersByOrderAsync(Guid orderUid, int page, int pageSize);
    Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid, int page, int pageSize);
    Task AddOfferAsync(Offer offer);
    Task UpdateOfferAsync(Offer offer);
    Task DeleteOfferAsync(Guid offerUid);
}
