using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Orders.Tests.RepositoryTests.Helpers;

namespace Rewriting.Services.Orders.Tests;

[TestClass]
public class CancelOrderTests
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
            CacheHelper.GetCacheStub(),
            _mapper
            );
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_DoneStatus_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.Done,
            PublishDate = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid
        };

        // Act
        await _orderService.CancelOrderAsync(cancelOrderModel);
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_CanceledStatus_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.Canceled,
            PublishDate = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid
        };

        // Act
        await _orderService.CancelOrderAsync(cancelOrderModel);
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_EvaluationResultStatus_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.Evaluation,
            PublishDate = DateTime.Parse("01.01.2020"),
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid
        };

        // Act
        await _orderService.CancelOrderAsync(cancelOrderModel);
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_OrderNotFound_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var unexistingOrderUid = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            PublishDate = DateTime.Parse("01.01.2020"),
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = unexistingOrderUid
        };

        // Act
        await _orderService.CancelOrderAsync(cancelOrderModel);
    }
} // TODO: Increase tests coverage
