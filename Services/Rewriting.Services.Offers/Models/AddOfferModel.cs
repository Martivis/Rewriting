namespace Rewriting.Services.Offers;

public class AddOfferModel
{
    public Guid OrderUid { get; set; }
    public Guid ContractorUid { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
}

