namespace Rewriting.Services.Offers;

public class OfferModel
{
    public Guid Uid { get; set; }
    public Guid ClientUid { get; set; }
    public Guid ContractorUid { get; set; }
    public Guid OrderUid { get; set; }
    public string ContractorName { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
    public DateTime PublishDate { get; set; }
}
