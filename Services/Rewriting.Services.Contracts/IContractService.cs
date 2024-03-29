﻿namespace Rewriting.Services.Contracts;

public interface IContractService
{
    Task<ClientAuthModel> GetClientAuthAsync(Guid contractUid);
    Task<ContractorAuthModel> GetContractorAuthAsync(Guid contractUid);
    Task<ContractDetailsModel> GetContractDetailsAsync(Guid contractUid);
    Task<IEnumerable<ContractModel>> GetContractsByUserAsync(Guid userUid, int page = 0, int pageSize = 10);
    Task<IEnumerable<ResultModel>> GetResultsAsync(Guid contractUid, int page = 0, int pageSize = 10);
    Task AddResultAsync(AddResultModel model);
    Task AcceptResultAsync(Guid contractUid);
    Task DeclineResultAsync(Guid contractUid);
    Task DeclineContractorAsync(Guid contractUid);
}