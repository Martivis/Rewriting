using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public interface IResultRepository
{
    Task<IEnumerable<ResultModel>> GetResultsByOrderAsync(Guid contractUid, int page = 0, int pageSize = 10);
    Task AddResult(Result result);
}
