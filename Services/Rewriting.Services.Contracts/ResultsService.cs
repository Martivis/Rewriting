using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Context.Entities;

namespace Rewriting.Services.Contracts;

public class ResultsService : IUncheckedResultsService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IMapper _mapper;
    
    public ResultsService(IDbContextFactory<AppDbContext> contextFactory, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ResultCompareModel>> GetResultsWithNullUniqueness()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var results = context.Set<Result>()
            .Where(result => result.Uniqueness == null)
            .OrderBy(result => result.PublishDate)
            .ToList();

        return _mapper.Map<IEnumerable<ResultCompareModel>>(results);
    }

    public async Task UpdateResultUniqueness(Guid resultUid, int uniqueness)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        await context.Set<Result>()
            .Where(r => r.Uid == resultUid)
            .ExecuteUpdateAsync(x => 
                x.SetProperty(r => r.Uniqueness, uniqueness));
    }
}