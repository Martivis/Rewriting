using System.Security.Claims;

namespace Rewriting.Services.Orders;

public class CancelOrderModel
{
    public Guid OrderUid { get; set; }
}
