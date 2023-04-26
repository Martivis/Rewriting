namespace Rewriting.WebApp;

public class UserContractsService : AbstractListService<ContractModel>
{
    public UserContractsService(IApiService apiService) : base(apiService)
    {
    }

    protected override string GetEndpoint()
    {
        return "v1/Contracts/GetContractsByUser";
    }
}
