using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public interface IOffersObservable
{
    event Action<OfferModel> OnOfferAdd;
    event Action<OfferModel> OnOfferAccept;
}
