
namespace Rewriting.WebApp;

public class OfferService
{
    private readonly IApiService _apiService;

    public OfferService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task AcceptOfferAsync(Guid offerUid)
    {
        await _apiService.PostDataAsync("v1/Offers/AcceptOffer", offerUid);
    }

    public async Task AddOfferAsync(AddOfferModel model)
    {
        await _apiService.PostDataAsync("v1/Offers/AddOffer", model);
    }
}