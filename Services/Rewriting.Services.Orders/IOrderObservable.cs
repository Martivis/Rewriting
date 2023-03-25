using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public interface IOrderObservable
{
    event Action<OrderDetailsModel> OnOrderAdd;
    event Action<Guid> OnOrderCancel;
    event Action<Guid> OnorderDelete;
}
