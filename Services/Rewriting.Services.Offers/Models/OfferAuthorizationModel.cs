using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public class OfferAuthorizationModel
{
    public Guid Uid { get; set; }
    public Guid ContractorUid { get; set; }
    public Guid ClientUid { get; set; }
}
