namespace Rewriting.Services.Orders;

public interface IOrderService
{
    Task<OrderModel> GetOrderAsync(Guid orderUid);
    Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page = 0, int pageSize = 10);
    Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page = 0, int pageSize = 10);
    Task<OrderDetailsModel> GetOrderDetailsAsync(Guid orderUid);
    Task<OrderDetailsModel> AddOrderAsync(AddOrderModel model);
    Task CancelOrderAsync(CancelOrderModel model);
    Task DeleteOrderAsync(Guid orderUid);
}