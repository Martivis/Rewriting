using System.Runtime.CompilerServices;

namespace Rewriting.Services.Orders;

public interface IOrderService
{
    Task<IEnumerable<OrderModel>> GetNewOrders(int page = 0, int pageSize = 10);
    Task<IEnumerable<OrderModel>> GetOrders(Guid userUid);
    Task<IEnumerable<OrderModel>> GetContracts(Guid userUid);
    Task<OrderDetailsModel> GetOrderDetails(Guid uid);

    Task<IEnumerable<OfferModel>> GetUserOffers(Guid userUid);
    Task<IEnumerable<OfferModel>> GetOrderOffers(Guid orderUid);

    Task<OrderDetailsModel> AddOrder(AddOrderModel model);
    Task<OfferModel> AddOffer(AddOfferModel model);
    Task AddContract(Guid offerGuid);
    Task AcceptResult(Guid orderUid);

    Task CancelOrder(Guid orderUid);
    Task DeclineResult(Guid contractUid);
    Task DeclineContractor(Guid contractUid);

    Task DeleteOrder(Guid orderUid);
}