namespace Rewriting.WebApp.Services;

public class OrderStub : IOrderService
{
    public async Task<IEnumerable<OrderModel>> GetOrdersAsync(int page, int pageSize)
    {
        return new List<OrderModel>()
        {
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "MyOrder",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "My New Order",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Mark Markson",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Admin Palych",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "MyOrder",
                Uid = Guid.NewGuid(),
            },new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title The Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "My Order",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "rewt34567",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Mark Markson",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Titlewerq",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Admin Palych",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "MyOrderwqt",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Perfect",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "MyOrder",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "qeryqery",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "My New Order",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Terry Markson",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Semen Palych",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "MyOrder",
                Uid = Guid.NewGuid(),
            },new OrderModel()
            {
                ClientName = "Wild Nature",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Title The Title",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "New User",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "My Order",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "The Name",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "rewt34567",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Mark Markson",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Querty",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Admin Palych",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Set Mode",
                Uid = Guid.NewGuid(),
            },
            new OrderModel()
            {
                ClientName = "Semen Semenych",
                PublishDate = DateTime.Now,
                ClientUid = Guid.NewGuid(),
                Status = 0,
                Title = "Set Mode",
                Uid = Guid.NewGuid(),
            },
        }
        .Skip(page * pageSize)
        .Take(pageSize)
        ;
    }
}
