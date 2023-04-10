using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public interface IContractObservable
{
    event Action<ContractDetailsModel> OnResultAdd;
    event Action<ContractDetailsModel> OnResultAccept;
    event Action<ContractDetailsModel> OnResultDecline;
    event Action<ContractDetailsModel> OnContractorDecline;
}