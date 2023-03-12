using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ContractDetailsModel
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public IEnumerable<string> Results { get; set; }
    public string Comment { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
    public Guid? ContractorUid { get; set; }
    public string? ContractorName { get; set; }
    public decimal Price { get; set; }
}
