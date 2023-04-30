using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

public interface IOrderRepository
{
    Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page = 0, int pageSize = 10);
    Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page = 0, int pageSize = 10);
    Task<Order> GetOrderAsync(Guid orderUid);
    Task<OrderDetailsModel> GetOrderDetailsAsync(Guid orderUid);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid orderUid);
}
