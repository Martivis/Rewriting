namespace Rewriting.WebApp;

public class OrderOffersService : AbstractListService<OfferModel>
{
    public OrderOffersService(HttpClient httpClient, WebAppSettings settings, IAuthService authService) 
        : base(httpClient, settings, authService)
    {
    }

    protected override string GetEndpointUrn()
    {
        return "v1/Offers/GetOffersByOrders";
    }
}
