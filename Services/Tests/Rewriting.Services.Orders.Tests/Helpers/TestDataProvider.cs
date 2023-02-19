using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.UnitTests;

internal static class TestDataProvider
{
    public static Guid User1Guid => Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    public static Guid User2Guid => Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

    public static ICollection<Order> Orders() =>
        new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.InProgress,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.Done,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            }
        };

    public static ICollection<OrderModel> OrderModels =>
        new List<OrderModel>
        {
            new OrderModel
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new OrderModel
            {
                Uid = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            }
        };
}
