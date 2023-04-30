using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public interface IContractRepository
{
    Task<Contract> GetContractAsync(Guid uid);
    Task<ContractDetailsModel> GetContractDetailsAsync(Guid uid);
    Task<IEnumerable<ContractModel>> GetContractsByUserAsync(Guid userUid, int page, int pageSize);
    Task AddContractAsync(Contract contract);
    Task UpdateContractAsync(Contract contract);
    Task DeleteContractAsync(Guid contractUid);
}
