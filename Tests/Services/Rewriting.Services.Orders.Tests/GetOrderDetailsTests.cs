using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Moq;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Tests;

[TestClass]
public class GetOrderDetailsTests
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

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new OrderDetailsModelProfile())));


        _orderService = new OrderService(
            _contextFactoryStub.Object,
            _mapper
            );
    }

    [TestMethod]
    [ExpectedException(typeof(ProcessException), "Exceprion was not thrown")]
    public async Task GetOrderDetails_NoExisting_ThrowsException()
    {
        // Act
        await _orderService.GetOrderDetailsAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    }

    [TestMethod]
    public async Task GetOrderDetails_Normal_Returns1elem()
    {
        // Arrange
        var orderUid = Guid.Parse("22222222-2222-2222-2222-222222222222");
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
                Uid = orderUid,
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

        var expected = new Guid("22222222-2222-2222-2222-222222222222");

        // Act
        var result = await _orderService.GetOrderDetailsAsync(orderUid);
        var actual = result.Uid;

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
