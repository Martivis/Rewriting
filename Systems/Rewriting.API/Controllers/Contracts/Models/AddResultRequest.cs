namespace Rewriting.API.Controllers.Contracts;

public class AddResultRequest
{
    public Guid ContractUid { get; set; }
    public string Text { get; set; }
}
