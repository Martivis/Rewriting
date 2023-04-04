using Moq;
using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders;
using AutoMapper;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Rewriting.Services.Orders.Tests;

[TestClass]
public class GetNewOrdersTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    private IMapper _mapper;
    
    private IOrderService _orderService;

    [TestInitialize]
    public void TestInitialize()
    {
        _contextHelper = new DbContextHelper();
        
        _contextFactoryStub = new Mock<IDbContextFactory<AppDbContext>>();
        _contextFactoryStub.Setup(method => 
            method.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(_contextHelper.Context));

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new OrderModelProfile())));

        _orderService = new OrderService(
            _contextFactoryStub.Object,
            _mapper
            );
    }

    [TestMethod]
    public async Task GetNewOrders_page0_pageSize2_Returns2elems()
    {
        // Arrange
        var orders = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
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
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(orders);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
        };

        // Act
        var result = await _orderService.GetNewOrdersAsync(0, 2);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetNewOrders_page1_pageSize2_Returns1elem()
    {
        // Arrange
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
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
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            new Guid("33333333-3333-3333-3333-333333333333"),
        };

        // Act
        var result = await _orderService.GetNewOrdersAsync(1, 2);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetNewOrders_NegativePage_ThrowsException()
    {
        // Act
        await _orderService.GetNewOrdersAsync(-1, 2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetNewOrders_ZeroPageSize_ThrowsException()
    {
        // Act
        await _orderService.GetNewOrdersAsync(0, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetNewOrders_NegativePageSize_ThrowsException()
    {
        // Act
        await _orderService.GetNewOrdersAsync(0, -1);
    }

    [TestMethod]
    public async Task GetNewOrders_DifferentStatuses_Returns2elems()
    {
        // Arrange
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Done,
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
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.InProgress,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Canceled,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            }
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("55555555-5555-5555-5555-555555555555"),
        };

        // Act
        var result = await _orderService.GetNewOrdersAsync();
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetNewOrders_DifferentClients_Returns2elems()
    {
        // Arrange
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
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
                ClientUid = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("22222222-2222-2222-2222-222222222222"),
        };

        // Act
        var result = await _orderService.GetNewOrdersAsync(0, 2);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

}