using System.Net.Http.Headers;
using System.Text.Json;

namespace Rewriting.WebApp;

public class OfferAcceptService
{
    private readonly IApiPostService _apiService;

    public OfferAcceptService(IApiPostService apiService)
    {
        _apiService = apiService;
    }

    public async Task AcceptOfferAsync(Guid offerUid)
    {
        await _apiService.PostDataAsync($"v1/Offers/AcceptOffer", offerUid);
    }
}