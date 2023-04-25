using System.Net.Http.Headers;
using System.Text.Json;

namespace Rewriting.WebApp;

public class OfferAcceptService : AbstractApiPostService<Guid>
{
    public OfferAcceptService(HttpClient httpClient, WebAppSettings settings, IAuthService authService)
        : base(httpClient, settings, authService)
    {
    }

    public async Task AcceptOfferAsync(Guid offerUid)
    {
        await PostDataAsync($"v1/Offers/AcceptOffer", offerUid);
    }
}