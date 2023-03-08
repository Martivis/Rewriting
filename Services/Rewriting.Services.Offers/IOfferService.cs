using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public interface IOfferService
{
    Task<IEnumerable<OfferModel>> GetOffersByOrder(Guid orderUid);
    Task<IEnumerable<OfferModel>> GetOffersByUser(Guid userUid);
    Task<OfferModel> AddOffer(AddOfferModel model);
}
