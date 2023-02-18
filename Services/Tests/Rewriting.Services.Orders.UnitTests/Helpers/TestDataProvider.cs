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
    public static IEnumerable<Order> Orders =>
        new List<Order>()
        {
            new Order
            {
                Uid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
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
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                ClientUid = User1Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
            new Order
            {
                Uid = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                ClientUid = User2Guid,
                Title = "TITLE",
                Comment = "COMMENT",
                Text = "TEXT",
                Status = OrderStatus.New,
                DateTime = DateTime.Parse("01.01.2020")
            },
        };
}
