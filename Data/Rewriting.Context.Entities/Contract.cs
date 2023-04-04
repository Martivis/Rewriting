using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class Contract : BaseEntity
{
    public decimal Price { get; set; }
    public virtual IEnumerable<Result> Result { get; set; }
    public Guid ContractorUid { get; set; }
    public virtual UserData Contractor { get; set; }
    public virtual Order Order { get; set; }
    public DateTime PublishDate { get; set; }
}
