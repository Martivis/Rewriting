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

namespace Rewriting.Services.Orders;

internal class OrderService : IOrderService
{
    private readonly IModelValidator<AddOrderModel> _addOrderValidator;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public OrderService(
        IModelValidator<AddOrderModel> addOrderValidator, 
        IDbContextFactory<AppDbContext> dbContextFactory,
        IMapper mapper)
    {
        _addOrderValidator = addOrderValidator;
        _contextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderModel>> GetNewOrders(int page = 0, int pageSize = 10)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var entities = await context.Set<Order>()
            .Where(order => order.Status == OrderStatus.New)
            .Skip(Math.Max(page * pageSize, 0))
            .Take(Math.Min(pageSize, 1000))
            .ToListAsync();

        var orders = entities.Select(_mapper.Map<OrderModel>);

        return orders;
    }

    public async Task<IEnumerable<OrderModel>> GetOrders(Guid userUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var orders = await context.Set<Order>()
            .Where(order => order.ClientUid == userUid)
            .Select(entity => _mapper.Map<OrderModel>(entity))
            .ToListAsync();

        return orders;
    }

    public async Task<OrderDetailsModel> GetOrderDetails(Guid uid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = context.Set<Order>().FindAsync(uid);

        return _mapper.Map<OrderDetailsModel>(order);
    }

    public async Task<OrderDetailsModel> AddOrder(AddOrderModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = _mapper.Map<Order>(model);
        order.Status = OrderStatus.New;
        order.DateTime = DateTime.Now;

        await context.AddAsync(order);
        context.SaveChanges();

        return _mapper.Map<OrderDetailsModel>(order);
    }

    public async Task DeleteOrder(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(orderUid)
            ?? throw new ProcessException($"Order {orderUid} not found");

        context.Remove(order);
        await context.SaveChangesAsync();
    }
}
