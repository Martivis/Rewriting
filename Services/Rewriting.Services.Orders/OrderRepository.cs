using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders
{
    internal class OrderRepository : IOrderRepository, IDisposable
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;

        private readonly AppDbContext _context;

        private const string OrderCachePrefix = "order_";

        public OrderRepository(IDbContextFactory<AppDbContext> contextFactory, ICacheService cache, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _cache = cache;
            _mapper = mapper;

            _context = _contextFactory.CreateDbContextAsync().Result;
        }

        public async Task<IEnumerable<OrderModel>> GetNewOrdersAsync(int page = 0, int pageSize = 10)
        {
            var orders = await _context.Set<Order>()
                .Where(order => order.Status == OrderStatus.New)
                .OrderByDescending(order => order.PublishDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderModel>>(orders);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserAsync(Guid userUid, int page = 0, int pageSize = 10)
        {
            var orders = await _context.Set<Order>()
                .Where(order => order.ClientUid == userUid)
                .OrderByDescending(order => order.PublishDate)
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderModel>>(orders);
        }

        public async Task<Order> GetOrderAsync(Guid orderUid)
        {
            return await _context.Set<Order>().FindAsync(orderUid)
                ?? throw new ProcessException($"Order {orderUid} not found");
        }

        public async Task<OrderDetailsModel> GetOrderDetailsAsync(Guid orderUid)
        {
            var orderDetails = await _cache.Get<OrderDetailsModel>($"{OrderCachePrefix}{orderUid}");
            if (orderDetails is null)
            {
                var order = await GetOrderAsync(orderUid);
                orderDetails = _mapper.Map<OrderDetailsModel>(order);
                await _cache.Set($"{OrderCachePrefix}{orderUid}", orderDetails);
            }
            return orderDetails;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Set<Order>().Attach(order);
            await _context.SaveChangesAsync();
            await _cache.Remove($"{OrderCachePrefix}{order.Uid}");
        }

        public async Task DeleteOrderAsync(Guid orderUid)
        {
            var order = await GetOrderAsync(orderUid);
            _context.Remove(order);
            await _context.SaveChangesAsync();
            await _cache.Remove($"{OrderCachePrefix}{order.Uid}");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
