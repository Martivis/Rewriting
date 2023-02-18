using Moq;
using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Common.Validator;

namespace Rewriting.Services.Orders.UnitTests;

[TestClass]
public class OrderServiceTests
{
    private DbContextHelper _contextHelper;
    private Mock<IDbContextFactory<AppDbContext>> _contextFactoryStub;
    
    private IOrderService _orderService;

    [ClassInitialize]
    public void ClassInitialize()
    {
        _contextHelper = new DbContextHelper();

        _contextFactoryStub = new Mock<IDbContextFactory<AppDbContext>>();
        _contextFactoryStub.Setup(method => 
            method.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(_contextHelper.Context));

        
    }

}