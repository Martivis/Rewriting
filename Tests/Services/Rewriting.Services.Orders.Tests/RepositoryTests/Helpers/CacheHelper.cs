using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Rewriting.Services.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Tests.RepositoryTests.Helpers;

internal static class CacheHelper
{
    public static ICacheService GetCacheStub()
    {
        var mock = new Mock<ICacheService>();

        mock.Setup(s => s.Get<OrderDetailsModel>(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        return mock.Object;
    }
}
