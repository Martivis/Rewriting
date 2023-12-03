namespace Rewriting.Services.Contracts;

public interface IUncheckedResultsService
{
    Task<IEnumerable<ResultCompareModel>> GetResultsWithNullUniquenessAsync(int limit = 10);
    Task UpdateResultUniquenessAsync(Guid resultUid, int uniqueness);
}