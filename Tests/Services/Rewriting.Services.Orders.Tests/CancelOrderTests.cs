using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rewriting.Common.Exceptions;
using Rewriting.Common.Security;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Tests;

[TestClass]
public class CancelOrderTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    private Mock<IMapper> _mapperStub;
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

        _mapperStub = new Mock<IMapper>();

        _authorizationServiceStub = new Mock<IAuthorizationService>();
        

        _orderService = new OrderService(
            _contextFactoryStub.Object,
            _mapperStub.Object,
            _authorizationServiceStub.Object
            );
    }

    [TestMethod]
    public async Task CancelOrder_CallsAuthorizeAsync()
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
            Status = OrderStatus.New,
            DateTime = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Success());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);

        // Assert
        _authorizationServiceStub.Verify(obj => 
            obj.AuthorizeAsync(
                claimsPrincipalStub.Object, 
                It.Is<Order>(o => o.Uid == orderUid), 
                AppScopes.OrdersEdit), Times.Once());
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_AuthorizationFailed_ThrowsProcessException()
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
            Status = OrderStatus.New,
            DateTime = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Failed());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_OrderNotFound_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var unexittingOrderUid = Guid.Parse("22222222-1111-1111-1111-111111111111");
        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.New,
            DateTime = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Success());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = unexittingOrderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);
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
            DateTime = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Success());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);
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
            DateTime = DateTime.Parse("01.01.2020")
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Success());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException))]
    public async Task CancelOrder_EvaluationResultStatus_ThrowsProcessException()
    {
        // Arrange
        var userUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var orderUid = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var contractorUid = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var order = new Order
        {
            Uid = orderUid,
            ClientUid = userUid,
            Title = "TITLE",
            Comment = "COMMENT",
            Text = "TEXT",
            Status = OrderStatus.Canceled,
            DateTime = DateTime.Parse("01.01.2020"),
            Contract = new Contract
            {
                ContractorUid = contractorUid,
                Price = 100,
                Result = new List<Result>
                {
                    new Result
                    {
                        Content = "Result",
                        Status = ResultStatus.Evaluation
                    }
                }
            }
        };

        _contextHelper.Context.Add(order);
        _contextHelper.Context.SaveChanges();

        _authorizationServiceStub.Setup(obj =>
            obj.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<Order>(),
                It.Is<string>(s => s == AppScopes.OrdersEdit)
                )
            ).ReturnsAsync(AuthorizationResult.Success());

        var claimsPrincipalStub = new Mock<ClaimsPrincipal>();

        var cancelOrderModel = new CancelOrderModel()
        {
            OrderUid = orderUid,
            Issuer = claimsPrincipalStub.Object
        };

        // Act
        await _orderService.CancelOrder(cancelOrderModel);
    }
}
