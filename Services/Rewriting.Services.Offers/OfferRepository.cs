using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rewriting.Common.Exceptions;
using Rewriting.Context;
using Rewriting.Context.Entities;
using Rewriting.Services.Cache;

namespace Rewriting.Services.Offers;

internal class OfferRepository : IOfferRepository, IDisposable
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    private readonly AppDbContext _context;

    private const string OrderCachePrefix = "order_";

    public OfferRepository(IDbContextFactory<AppDbContext> contextFactory, ICacheService cache, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _cache = cache;
        _mapper = mapper;

        _context = _contextFactory.CreateDbContext();
    }

    public async Task<Offer> GetOfferAsync(Guid offerUid)
    {
        return await _context.Set<Offer>().FindAsync(offerUid)
            ?? throw new ProcessException($"Offer {offerUid} not found");
    }

    public async Task<IEnumerable<OfferModel>> GetOffersByOrderAsync(Guid orderUid, int page, int pageSize)
    {
        var offers = await _context.Set<Offer>()
                 .Where(offer => offer.OrderUid == orderUid)
                 .OrderByDescending(offer => offer.PublishDate)
                 .Skip(pageSize * page)
                 .Take(pageSize)
                 .ToListAsync();

        return _mapper.Map<IEnumerable<OfferModel>>(offers);
    }

    public async Task<IEnumerable<OfferModel>> GetOffersByUserAsync(Guid userUid, int page, int pageSize)
    {
        var offers = await _context.Set<Offer>()
                .Where(offer => offer.ContractorUid == userUid)
                .OrderByDescending(offer => offer.PublishDate)
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToListAsync();

        return _mapper.Map<IEnumerable<OfferModel>>(offers);
    }

    public async Task AddOfferAsync(Offer offer)
    {
        await _context.AddAsync(offer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOfferAsync(Offer offer)
    {
        _context.Update(offer);
        await _context.SaveChangesAsync();

        await _cache.Remove($"{OrderCachePrefix}{offer.OrderUid}");
    }

    public async Task DeleteOfferAsync(Guid offerUid)
    {
        var offer = GetOfferAsync(offerUid);
        _context.Remove(offer);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
