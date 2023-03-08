using AutoMapper;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Offers;

public class OfferModel
{
    public Guid Uid { get; set; }
    public Guid ContractorUid { get; set; }
    public string ContractorName { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
}
