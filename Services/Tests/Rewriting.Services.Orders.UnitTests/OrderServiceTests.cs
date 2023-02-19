using Moq;
using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Orders.UnitTests;

[TestClass]
public class OrderServiceTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    
    private IOrderService _orderService;

    [TestInitialize]
    public void TestInitialize()
    {
        _contextHelper = new DbContextHelper();
        
        _contextFactoryStub = new Mock<IDbContextFactory<AppDbContext>>();
        _contextFactoryStub.Setup(method => 
            method.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(_contextHelper.Context));

        _orderService = new OrderService();
    }

    [TestMethod]
    public async Task GetNewOrders_page0_pageSize_2_Returns2elems()
    {
        var data = new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.InProgress,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Done,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
        };

        _contextHelper.Context.AddRange(data);
        _contextHelper.Context.SaveChanges();

        var expected = new List<OrderModel>
        {
            new OrderModel
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            }
        };

        var actual = await _orderService.GetNewOrders();

        CollectionAssert.AreEqual(expected, actual);
    }
}