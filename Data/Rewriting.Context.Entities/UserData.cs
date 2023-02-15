using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class UserData : BaseEntity
{
    public virtual UserIdentity UserIdentity { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public uint OrdersCount { get; set; }
    public uint CompletedContractsCount { get; set; }
    public virtual ICollection<Offer> Offers { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; }
}
