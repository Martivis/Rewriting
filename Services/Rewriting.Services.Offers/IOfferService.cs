namespace Rewriting.Services.Offers;

public interface IOfferService
{
    Task<OfferAuthorizationModel> GetOfferAuthAsync(Guid offerUid);
    Task<IEnumerable<OfferModel>> GetOffersByOrderAsync(Guid orderUid);
    Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid);
    Task<OfferModel> AddOfferAsync(AddOfferModel model);
    Task AcceptOfferAsync(Guid offerUid);
}
