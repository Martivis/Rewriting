using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class Offer : BaseEntity
{
    public Guid ContractorUid { get; set; }
    public virtual UserData Contractor { get; set; }
    public Guid OrderUid { get; set; }
    public virtual Order Order { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
}
