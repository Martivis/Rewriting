using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ContractModel
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
}
