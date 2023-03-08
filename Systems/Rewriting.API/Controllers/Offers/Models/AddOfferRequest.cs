namespace Rewriting.API.Controllers.Offers;

public class AddOfferRequest
{ 
    public Guid OrderUid { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
}
