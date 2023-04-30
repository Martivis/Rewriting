using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;

namespace Rewriting.Services.Contracts;

internal class ResultRepository : IResultRepository, IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    private readonly AppDbContext _context;

    private const string OrderCachePrefix = "order_";

    public ResultRepository(IDbContextFactory<AppDbContext> contextFactory, ICacheService cache, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _cache = cache;
        _mapper = mapper;

        _context = _contextFactory.CreateDbContext();
    }

    public async Task<IEnumerable<ResultModel>> GetResultsByOrderAsync(Guid contractUid, int page = 0, int pageSize = 10)
    {
        var results = await _context.Set<Result>()
            .Where(result => result.ContractUid == contractUid)
            .OrderByDescending(result => result.PublishDate)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ResultModel>>(results);
    }

    public async Task AddResult(Result result)
    {
        _context.Add(result);
        await _context.SaveChangesAsync();

        await _cache.Remove($"{OrderCachePrefix}{result.ContractUid}");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
