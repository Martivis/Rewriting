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
    private readonly IModelValidator<AddOfferModel> _addOfferValidator;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;

    public OrderService(
        IModelValidator<AddOrderModel> addOrderValidator, 
        IModelValidator<AddOfferModel> addOfferValidator,
        IDbContextFactory<AppDbContext> dbContextFactory,
        IMapper mapper)
    {
        _addOrderValidator = addOrderValidator;
        _addOfferValidator = addOfferValidator;
        _contextFactory = dbContextFactory;
        _mapper = mapper;
    }

    #region Orders

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

    public async Task<IEnumerable<OrderModel>> GetContracts(Guid userUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var orders = await context.Set<Order>()
            .Where(order => order.Contract != null && order.Contract.ContractorUid == userUid)
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

    public async Task CancelOrder(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(orderUid)
                ?? throw new ProcessException($"Order {orderUid} not found");

        if (order.Contract?.Result is not null)
            throw new ProcessException("Unable to cancel order with attacher result");

        order.Status = OrderStatus.Canceled;
        context.Update(order);
        await context.SaveChangesAsync();
    }

    public async Task DeleteOrder(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(orderUid)
            ?? throw new ProcessException($"Order {orderUid} not found");

        context.Remove(order);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Offers
    public async Task<IEnumerable<OfferModel>> GetOrderOffers(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var offers = await context.Set<Offer>()
            .Where(offer => offer.OrderUid == orderUid)
            .Select(entity => _mapper.Map<OfferModel>(entity))
            .ToListAsync();

        return offers;
    }

    public async Task<IEnumerable<OfferModel>> GetUserOffers(Guid userUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var offers = await context.Set<Offer>()
            .Where(offer => offer.ContractorUid == userUid)
            .Select(entity => _mapper.Map<OfferModel>(entity))
            .ToListAsync();

        return offers;
    }

    public async Task<OfferModel> AddOffer(AddOfferModel model)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(model.OrderUid)
            ?? throw new ProcessException($"Order {model.OrderUid} not found");
        if (order.Status != OrderStatus.New)
            throw new ProcessException("Unable to add offer");

        var offer = _mapper.Map<Offer>(model);
        await context.AddAsync(offer);
        context.SaveChanges();

        return _mapper.Map<OfferModel>(offer);
    }
    #endregion

    public async Task AddContract(Guid offerUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var offer = await context.Set<Offer>()
            .FirstOrDefaultAsync(o => o.Uid == offerUid)
            ?? throw new ProcessException($"Offer {offerUid} not found");

        if (offer.Order.Status != OrderStatus.New)
            throw new ProcessException($"Unable to add contract to order {offer.OrderUid}");

        offer.Order.Status = OrderStatus.InProgress;

        var contract = _mapper.Map<Contract>(offer);
        await context.AddAsync(contract);
        await context.SaveChangesAsync();
    }

    public async Task AcceptResult(Guid orderUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Set<Order>().FindAsync(orderUid)
            ?? throw new ProcessException($"Order {orderUid} not found");

        if (order.Contract?.Result is null)
            throw new ProcessException($"Result for order {orderUid} not found");
        if (order.Status != OrderStatus.InProgress)
            throw new ProcessException($"Unable to accept result for order {orderUid}");

        order.Contract.Result.Last().Status = ResultStatus.Accepted;
        order.Status = OrderStatus.Done;
        await context.SaveChangesAsync();
    }

    public async Task DeclineContractor(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        if (contract.Order.Status != OrderStatus.InProgress)
            throw new ProcessException("Unable to decline contractor");

        context.Remove(contract);
        await context.SaveChangesAsync();
    }

    public async Task DeclineResult(Guid contractUid)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var contract = await context.Set<Contract>().FindAsync(contractUid)
            ?? throw new ProcessException($"Contract {contractUid} not found");

        if (contract.Order.Status != OrderStatus.InProgress)
            throw new ProcessException("Unable to decline result");
        if (contract.Result.Last().Status != ResultStatus.Evaluation)
            throw new ProcessException("Unable to decline result");

        contract.Result.Last().Status = ResultStatus.Declined;
        await context.SaveChangesAsync();
    }

}
