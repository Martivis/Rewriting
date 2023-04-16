namespace Rewriting.WebApp.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderModel>> GetOrdersAsync(int page, int pageSize);
}
