namespace Rewriting.WebApp;

public class OrderOffersService : AbstractListService<OfferModel>
{
    public OrderOffersService(HttpClient httpClient, WebAppSettings settings, IAuthService authService) 
        : base(httpClient, settings, authService)
    {
    }

    public Guid OrderUid { get; set; }

    protected override string GetEndpoint()
    {
        return "v1/Offers/GetOffersByOrder";
    }

    protected override string GetAdditionalParameters()
    {
        return "orderUid=" + OrderUid.ToString();
    }
}
