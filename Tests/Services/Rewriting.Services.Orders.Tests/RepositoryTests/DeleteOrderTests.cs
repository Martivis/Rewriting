﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders.Tests.RepositoryTests.Helpers;

namespace Rewriting.Services.Orders.Tests.RepositoryTests;

[TestClass]
public class DeleteOrderTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    private IMapper _mapper;

    private IOrderRepository _orderRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        _contextHelper = new DbContextHelper();

        _contextFactoryStub = new Mock<IDbContextFactory<AppDbContext>>();
        _contextFactoryStub.Setup(method =>
            method.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(_contextHelper.Context));

        _mapper = new Mapper(new MapperConfiguration(_ => { }));

        _orderRepository = new OrderRepository(
            _contextFactoryStub.Object,
            CacheHelper.GetCacheStub(),
            _mapper
            );
    }

    [TestCleanup]
    public void TestCleanup()
    {
        (_orderRepository as IDisposable)!.Dispose();
    }

    [TestMethod]
    public async Task DeleteOrder_OneOrder()
    {
        var context = _contextHelper.Context;

        // Arrange

        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var orders = new List<Order>()
        {
            new Order
            {
                Uid = orderUid,
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            }
        };

        context.AddRange(orders);
        context.SaveChanges();
        foreach (var order in orders)
        {
            context.Entry(order).State = EntityState.Detached;
        }

        var expected = new List<Guid>()
        {
            Guid.Parse("22222222-2222-2222-2222-222222222222")
        };

        // Act

        await _orderRepository.DeleteOrderAsync(orderUid);

        // Assert

        var actual = context.Orders.Select(o => o.Uid).ToList();

        CollectionAssert.AreEqual(expected, actual);
    }
}
