namespace Rewriting.WebApp;

public class ContractResultsService : AbstractListService<ResultModel>
{
    public ContractResultsService(IApiService apiService) : base(apiService)
    {
    }

    public Guid ContractUid { get; set; }

    protected override string GetAdditionalParameters()
    {
        return $"contractUid={ContractUid}";
    }

    protected override string GetEndpoint()
    {
        return "v1/Contracts/GetResults";
    }
}
