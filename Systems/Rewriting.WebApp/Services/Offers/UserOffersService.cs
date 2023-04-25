namespace Rewriting.WebApp;

public class UserOffersService : AbstractListService<OfferModel>
{
    public UserOffersService(IApiService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Offers/GetOffersByUser";
    }
}
