using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Tests;

[TestClass]
public class GetOrdersByUserTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    private IMapper _mapper;
    private Mock<IAuthorizationService> _authorizationServiceStub;

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
    public async Task GetOrdersByUser_page0_pageSize2_Returns2elems()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var data = new List<Order>()
    {
        new Order
        {
            Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            PublishDate = DateTime.Parse("01.01.2020")
        },
        new Order
        {
            Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            PublishDate = DateTime.Parse("01.01.2020")
        },
        new Order
        {
            Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            ClientUid = userUid,
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
        var result = await _orderService.GetOrdersByUserAsync(userUid, 0, 2);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetOrdersByUser_page1_pageSize2_Returns1elem()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var data = new List<Order>()
    {
        new Order
        {
            Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            PublishDate = DateTime.Parse("01.01.2020")
        },
        new Order
        {
            Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            PublishDate = DateTime.Parse("01.01.2020")
        },
        new Order
        {
            Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            ClientUid = userUid,
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
        var result = await _orderService.GetOrdersByUserAsync(userUid, 1, 2);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetOrdersByUser_NegativePage_ThrowsException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

        // Act
        await _orderService.GetOrdersByUserAsync(userUid, -1, 2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetOrdersByUser_ZeroPageSize_ThrowsException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

        // Act
        await _orderService.GetOrdersByUserAsync(userUid, 0, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException), "Exceprion was not thrown")]
    public async Task GetOrdersByUser_NegativePageSize_ThrowsException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

        // Act
        await _orderService.GetOrdersByUserAsync(userUid, 0, -1);
    }

    [TestMethod]
    public async Task GetOrdersByUser_DifferentStatuses_Returns4elems()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = userUid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = userUid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.InProgress,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = userUid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Done,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ClientUid = userUid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Canceled,
                PublishDate = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("33333333-3333-3333-3333-333333333333"),
            new Guid("44444444-4444-4444-4444-444444444444"),
        };

        // Act
        var result = await _orderService.GetOrdersByUserAsync(userUid);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert

        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task GetOrdersByUser_DifferentClients_Returns2elems()
    {
        // Arrange
        var user1Uid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var user2Uid = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = user1Uid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = user2Uid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.InProgress,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = user1Uid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Done,
                PublishDate = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ClientUid = user2Uid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Canceled,
                PublishDate = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<Guid>
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("33333333-3333-3333-3333-333333333333"),
        };

        // Act
        var result = await _orderService.GetOrdersByUserAsync(user1Uid);
        var actual = result.Select(x => x.Uid).ToList();

        // Assert

        CollectionAssert.AreEqual(expected, actual);
    }
}
