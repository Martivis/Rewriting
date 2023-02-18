using System.Runtime.CompilerServices;

namespace Rewriting.Services.Orders;

public interface IOrderService
{
    Task<List<OrderModel>> GetNewOrders(int page = 0, int pageSize = 10);
    Task<List<OrderModel>> GetOrdersByUser(Guid userUid, int page = 0, int pageSize = 10);
    Task<OrderDetailsModel> GetOrderDetails(Guid uid);
    Task<OrderDetailsModel> AddOrder(AddOrderModel model);
    Task DeleteOrder(Guid orderUid);
}