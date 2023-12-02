namespace Rewriting.Services.Contracts;

public interface IUncheckedResultsService
{
    Task<IEnumerable<ResultCompareModel>> GetResultsWithNullUniquenessAsync();
    Task UpdateResultUniquenessAsync(Guid resultUid, int uniqueness);
}