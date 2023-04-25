namespace Rewriting.WebApp;

public class OrderOffersService : AbstractListService<OfferModel>
{
    public OrderOffersService(IApiService apiService) : base(apiService)
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
