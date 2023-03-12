using System.ComponentModel;

namespace Rewriting.Services.Contracts;

public interface IContractService
{
    Task<ContractModel> GetContract(Guid contractUid);
    Task<ContractDetailsModel> GetContractDetails(Guid contractUid);
    Task<IEnumerable<ContractModel>> GetContractsByUser(Guid userUid);
    Task AddResult(AddResultModel model);
    Task AcceptResult(Guid contractUid);
    Task DeclineResult(Guid contractUid);
    Task DeclineContractor(Guid contractUid);
}