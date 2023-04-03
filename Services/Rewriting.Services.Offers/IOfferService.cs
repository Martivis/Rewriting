namespace Rewriting.Services.Offers;

public interface IOfferService
{
    Task<OfferAuthorizationModel> GetOfferAuthAsync(Guid offerUid);
    Task<IEnumerable<OfferModel>> GetOffersByOrderAsync(Guid orderUid, int page = 0, int pageSize = 10);
    Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid, int page = 0, int pageSize = 10);
    Task<OfferModel> AddOfferAsync(AddOfferModel model);
    Task AcceptOfferAsync(Guid offerUid);
}
