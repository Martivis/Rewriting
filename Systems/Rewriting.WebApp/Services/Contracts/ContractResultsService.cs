namespace Rewriting.WebApp;

public class ContractResultsService : AbstractListService<ResultModel>
{
    public ContractResultsService(IApiService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Contracts/GetResults";
    }
}
