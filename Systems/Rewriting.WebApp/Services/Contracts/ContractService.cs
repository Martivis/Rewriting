namespace Rewriting.WebApp;

public class ContractService
{
    private readonly IApiService _apiService;

    public ContractService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<ContractDetailsModel> GetContractDetailsAsync(Guid uid)
    {
        return await _apiService.GetDataAsync<ContractDetailsModel>($"v1/Contracts/GetContractDetails?contractUid={uid}");
    }

    public async Task AddResultAsync(AddResultModel model)
    {
        await _apiService.PostDataAsync("v1/Contracts/AddResult", model);
    }

    public async Task AcceptResultAsync(Guid contractUid)
    {
        await _apiService.PatchDataAsync("v1/Contracts/AcceptResult", new { contractUid });
    }

    public async Task DeclineResultAsync(Guid contractUid)
    {
        await _apiService.PatchDataAsync("v1/Contracts/DeclineResult", new { contractUid });
    }

    public async Task DeclineContractorAsync(Guid contractUid)
    {
        await _apiService.PatchDataAsync("v1/Contracts/DeclineContractor", new { contractUid });
    }
}
