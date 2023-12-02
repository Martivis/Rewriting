namespace Rewriting.Services.Contracts;

public interface IUncheckedResultsService
{
    Task<IEnumerable<ResultCompareModel>> GetResultsWithNullUniqueness();
    Task UpdateResultUniqueness(Guid resultUid, int uniqueness);
}