﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class Order : BaseEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
    public DateTime PublishDate { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public virtual UserData Client { get; set; }
    public virtual ICollection<Offer> Offers { get; set; }
    public virtual Contract? Contract { get; set; }

}
