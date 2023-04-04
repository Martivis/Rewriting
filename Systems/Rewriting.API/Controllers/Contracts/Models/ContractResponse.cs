﻿using Rewriting.Context.Entities;

namespace Rewriting.API.Controllers.Contracts;

public class ContractResponse
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Price { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
}
