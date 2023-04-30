using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;

namespace Rewriting.Services.Contracts;

internal class ContractRepository : IContractRepository, IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    private readonly AppDbContext _context;

    private const string OrderCachePrefix = "order_";

    public ContractRepository(IDbContextFactory<AppDbContext> contextFactory, ICacheService cache, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _cache = cache;
        _mapper = mapper;

        _context = _contextFactory.CreateDbContext();
    }

    public async Task<Contract> GetContractAsync(Guid uid)
    {
        return await _context.Set<Contract>().FindAsync(uid)
            ?? throw new ProcessException($"Contract {uid} not found");
    }

    public async Task<ContractDetailsModel> GetContractDetailsAsync(Guid uid)
    {
        var contract = await GetContractAsync(uid);

        var order = contract.Order;

        return _mapper.Map<ContractDetailsModel>(order);
    }

    public async Task<IEnumerable<ContractModel>> GetContractsByUserAsync(Guid userUid, int page, int pageSize)
    {
        var contracts = await _context.Set<Order>()
            .Where(order => order.Contract != null && order.Contract.ContractorUid == userUid)
            .OrderByDescending(order => order.Contract!.PublishDate)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ContractModel>>(contracts);
    }

    public async Task AddContractAsync(Contract contract)
    {
        await _context.AddAsync(contract);
        await _context.SaveChangesAsync();

        await _cache.Remove($"{OrderCachePrefix}{contract.Uid}");
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        _context.Update(contract);
        await _context.SaveChangesAsync();

        await _cache.Remove($"{OrderCachePrefix}{contract.Uid}");
    }

    public async Task DeleteContractAsync(Guid contractUid)
    {
        var contract = await GetContractAsync(contractUid);

        _context.Remove(contract);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
