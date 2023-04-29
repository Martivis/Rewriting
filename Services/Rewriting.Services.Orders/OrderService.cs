using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;

namespace Rewriting.Services.Orders;

internal class OrderService : IOrderService, IOrderObservable
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public event Action<OrderDetailsModel> OnOrderAdd;
    public event Action<OrderDetailsModel> OnOrderCancel;
    public event Action<OrderDetailsModel> OnorderDelete;

    public OrderService(
        IOrderRepository orderRepository,
        IMapper mapper
        )
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get order with specified Uid
    /// </summary>
    /// <param name="orderUid">Order's Uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order with specified Uid doesn't exist in database</exception>
    public async Task<OrderModel> GetOrderAsync(Guid orderUid)
    {
        var order = await _orderRepository.GetOrderAsync(orderUid);

        return _mapper.Map<OrderModel>(order);
    }

    /// <summary>
    /// Get orders with status 'New'
    /// </summary>
    /// <param name="page">Page number (starting with 0)</param>
    /// <param name="pageSize">Orders number per page</param>
    /// <returns>A Task containing an IEnumerable of OrderModel objects</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when page value < 0 or pageSize value < 1</exception>
    public async Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page = 0, int pageSize = 10)
    {
        if (page < 0)
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");

        var orders = await _orderRepository.GetNewOrdersAsync(page, pageSize);

        return _mapper.Map<IEnumerable<OrderModel>>(orders);
    }

    /// <summary>
    /// Get orders placed by specified user
    /// </summary>
    /// <param name="userUid">User's uid</param>
    /// <param name="page">Page number (starting with 0)</param>
    /// <param name="pageSize">Orders number per page</param>
    /// <returns>A Task containing an IEnumerable of OrderModel objects</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when page value < 0 or pageSize value < 1</exception>
    public async Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
    {
        if (page < 0)
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");

        var orders = await _orderRepository.GetOrdersByUserAsync(userUid, page, pageSize);

        return _mapper.Map<IEnumerable<OrderModel>>(orders);
    }

    /// <summary>
    /// Get delailed information about specified order
    /// </summary>
    /// <param name="uid">Order's uid</param>
    /// <returns>A Task containing an OrderDetailsModel object</returns>
    public async Task<OrderDetailsModel> GetOrderDetailsAsync(Guid uid)
    {
        var orderDetails = await _orderRepository.GetOrderDetailsAsync(uid);

        return orderDetails;
    }

    /// <summary>
    /// Add new order
    /// </summary>
    /// <param name="model">A model of new order</param>
    /// <returns>A Task containing an OrderDetailsModel object</returns>
    public async Task<OrderDetailsModel> AddOrderAsync(AddOrderModel model)
    {
        var order = _mapper.Map<Order>(model);
        order.Status = OrderStatus.New;
        order.PublishDate = DateTime.UtcNow;

        await _orderRepository.AddOrderAsync(order);

        var orderDetailsModel = _mapper.Map<OrderDetailsModel>(order);

        OnOrderAdd?.Invoke(orderDetailsModel);

        return orderDetailsModel;
    }

    /// <summary>
    /// Cancel specified order
    /// </summary>
    /// <param name="orderUid">Order's Uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order is not cancalable</exception>
    public async Task CancelOrderAsync(CancelOrderModel model)
    {
        var order = await _orderRepository.GetOrderAsync(model.OrderUid);

        if (!IsCancelable(order))
            throw new ProcessException($"Unable to cancel order {order.Uid}");

        order.Status = OrderStatus.Canceled;
        
        await _orderRepository.UpdateOrderAsync(order);

        var orderDetails = _mapper.Map<OrderDetailsModel>(order);
        OnOrderCancel?.Invoke(orderDetails);
    }

    private static bool IsCancelable(Order order)
    {
        return
            order.Status != OrderStatus.Canceled &&
            order.Status != OrderStatus.Done &&
            order.Status != OrderStatus.Evaluation;
    }

    /// <summary>
    /// Delete specified order
    /// </summary>
    /// <param name="orderUid">Order's uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order with specified Uid doesn't exist in database</exception>
    public async Task DeleteOrderAsync(Guid orderUid)
    {
        var orderDetails = await _orderRepository.GetOrderDetailsAsync(orderUid);

        await _orderRepository.DeleteOrderAsync(orderUid);

        OnorderDelete?.Invoke(orderDetails);
    }
}
