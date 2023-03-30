using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public interface IOrderObservable
{
    event Action<OrderDetailsModel> OnOrderAdd;
    event Action<OrderDetailsModel> OnOrderCancel;
    event Action<OrderDetailsModel> OnorderDelete;
}
