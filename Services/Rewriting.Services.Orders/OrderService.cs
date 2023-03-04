using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Validator;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Rewriting.Common.Security;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace Rewriting.Services.Orders;

internal class OrderService : IOrderService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public OrderService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        IMapper mapper
        )
    {
        _contextFactory = dbContextFactory;
        _mapper = mapper;
    }

    /// <summary>
    /// Get order with specified Uid
    /// </summary>
    /// <param name="orderUid">Order's Uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order with specified Uid doesn't exist in database</exception>
    public async Task<OrderModel> GetOrder(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var order = await context.Set<Order>().FindAsync(orderUid)
            ?? throw new ProcessException($"Order {orderUid} not found");

        return _mapper.Map<OrderModel>(order);
    }

    /// <summary>
    /// Get orders with status 'New'
    /// </summary>
    /// <param name="page">Page number (starting with 0)</param>
    /// <param name="pageSize">Orders number per page</param>
    /// <returns>A Task containing an List of OrderModel objects</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when page value < 0 or pageSize value < 1</exception>
    public async Task<List<OrderModel>> GetNewOrders(int page = 0, int pageSize = 10)
    {
        if (page < 0)
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");

        using var context = await _contextFactory.CreateDbContextAsync();

        var orders = await context.Set<Order>()
            .Where(order => order.Status == OrderStatus.New)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<List<OrderModel>>(orders);
    }

    /// <summary>
    /// Get orders placed by specified user
    /// </summary>
    /// <param name="userUid">User's uid</param>
    /// <param name="page">Page number (starting with 0)</param>
    /// <param name="pageSize">Orders number per page</param>
    /// <returns>A Task containing an List of OrderModel objects</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when page value < 0 or pageSize value < 1</exception>
    public async Task<List<OrderModel>> GetOrdersByUser(Guid userUid, int page = 0, int pageSize = 10)
    {
        if (page < 0)
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be greater than zero");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size should be greater than zero");

        using var context = await _contextFactory.CreateDbContextAsync();

        var orders = await context.Set<Order>()
            .Where(order => order.ClientUid == userUid)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<List<OrderModel>>(orders);
    }

    /// <summary>
    /// Get delailed information about specified order
    /// </summary>
    /// <param name="uid">Order's uid</param>
    /// <returns>A Task containing an OrderDetailsModel object</returns>
    public async Task<OrderDetailsModel> GetOrderDetails(Guid uid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(uid)
            ?? throw new ProcessException($"Order {uid} not found");

        return _mapper.Map<OrderDetailsModel>(order);
    }

    /// <summary>
    /// Add new order
    /// </summary>
    /// <param name="model">A model of new order</param>
    /// <returns>A Task containing an OrderDetailsModel object</returns>
    public async Task<OrderDetailsModel> AddOrder(AddOrderModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = _mapper.Map<Order>(model);
        order.Status = OrderStatus.New;
        order.DateTime = DateTime.UtcNow;

        await context.AddAsync(order);
        context.SaveChanges();
        
        return _mapper.Map<OrderDetailsModel>(order);
    }

    /// <summary>
    /// Cancel specified order
    /// </summary>
    /// <param name="orderUid">Order's Uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order is not cancalable</exception>
    public async Task CancelOrder(CancelOrderModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = _mapper.Map<Order>(model.OrderModel);

        context.Attach(order);

        if (!IsCancelable(order))
            throw new ProcessException($"Unable to cancel order {order.Uid}");

        order.Status = OrderStatus.Canceled;

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete specified order
    /// </summary>
    /// <param name="orderUid">Order's uid</param>
    /// <returns></returns>
    /// <exception cref="ProcessException">Thrown when order with specified Uid doesn't exist in database</exception>
    public async Task DeleteOrder(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(orderUid)
            ?? throw new ProcessException($"Order {orderUid} not found");

        context.Remove(order);
        await context.SaveChangesAsync();
    }

    private static bool IsCancelable(Order order)
    {
        return
            order.Status != OrderStatus.Canceled &&
            order.Status != OrderStatus.Done &&
            (
                order.Contract is null ||
                !order.Contract.Result.Any(e => e.Status == ResultStatus.Evaluation)
            );
    }
}
