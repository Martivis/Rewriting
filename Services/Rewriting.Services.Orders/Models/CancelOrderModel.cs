using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class CancelOrderModel
{
    public OrderModel OrderModel { get; set; }
    public ClaimsPrincipal Issuer { get; set; }
}
