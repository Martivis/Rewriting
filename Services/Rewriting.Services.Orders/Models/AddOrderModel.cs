﻿namespace Rewriting.Services.Orders;

public class AddOrderModel
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
    public Guid ClientUid { get; set; }
}
