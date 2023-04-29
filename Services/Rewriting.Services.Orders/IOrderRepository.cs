using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders;

internal interface IOrderRepository
{
    Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page, int pageSize);
    Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page, int pageSize);
    Task<Order> GetOrderAsync(Guid orderUid);
    Task<OrderDetailsModel> GetOrderDetailsAsync(Guid orderUid);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid orderUid);
}
