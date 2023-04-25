﻿namespace Rewriting.WebApp;

public class OrderDetailsModel
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ContractPublishDate { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ClientUid { get; set; }
    public string ClientName { get; set; }
    public Guid? ContractorUid { get; set; }
    public string? ContractorName { get; set; }
}