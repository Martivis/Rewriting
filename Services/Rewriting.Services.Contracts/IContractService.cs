using System.ComponentModel;

namespace Rewriting.Services.Contracts;

public interface IContractService
{
    Task<ClientAuthModel> GetClientAuth(Guid contractUid);
    Task<ContractorAuthModel> GetContractorAuth(Guid contractUid);
    Task<ContractDetailsModel> GetContractDetails(Guid contractUid);
    Task<IEnumerable<ContractModel>> GetContractsByUser(Guid userUid);
    Task AddResult(AddResultModel model);
    Task AcceptResult(Guid contractUid);
    Task DeclineResult(Guid contractUid);
    Task DeclineContractor(Guid contractUid);
}