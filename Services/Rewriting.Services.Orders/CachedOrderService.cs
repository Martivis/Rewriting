using Microsoft.Extensions.Caching.Distributed;
using Rewriting.Context.Entities;
using System.Text.Json;

namespace Rewriting.Services.Orders;

internal class CachedOrderService : IOrderService
{
    private readonly OrderService _orderService;
    IDistributedCache _cache;

    public CachedOrderService(OrderService orderService, IDistributedCache cache)
    {
        _orderService = orderService;
        _cache = cache;
    }

    public async Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page = 0, int pageSize = 10)
    {
        return await _orderService.GetNewOrdersAsync(page, pageSize);
    }

    public async Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
    {
        return await GetOrdersByUserAsync(userUid, page, pageSize);
    }

    public async Task<OrderDetailsModel> GetOrderDetailsAsync(Guid orderUid)
    {
        var cacheKey = $"{orderUid}";

        var orderString = await _cache.GetStringAsync(cacheKey);

        if (orderString is not null)
            return JsonSerializer.Deserialize<OrderDetailsModel>(orderString) ?? new OrderDetailsModel();

        var order = await _orderService.GetOrderDetailsAsync(orderUid);
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(order), new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10)
        });
        return order;
    }

    public async Task<OrderModel> GetOrderAsync(Guid orderUid)
    {
        return await _orderService.GetOrderAsync(orderUid);
    }

    public async Task<OrderDetailsModel> AddOrderAsync(AddOrderModel model)
    {
        return await _orderService.AddOrderAsync(model);
    }

    public async Task CancelOrderAsync(CancelOrderModel model)
    {
        await _orderService.CancelOrderAsync(model);
    }

    public async Task DeleteOrderAsync(Guid orderUid)
    {
        await _orderService.DeleteOrderAsync(orderUid);
    }
}
