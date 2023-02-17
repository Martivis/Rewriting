using System.Runtime.CompilerServices;

namespace Rewriting.Services.Orders;

public interface IOrderService
{
    Task<IEnumerable<OrderModel>> GetNewOrders(int page = 0, int pageSize = 10);
    Task<IEnumerable<OrderModel>> GetOrders(Guid userUid);
    Task<OrderDetailsModel> GetOrderDetails(Guid uid);
    Task<OrderDetailsModel> AddOrder(AddOrderModel model);
    Task DeleteOrder(Guid orderUid);
}