using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using Rewriting.Services.Contracts;
using Rewriting.Services.TextComparer;

namespace Rewriting.TextComparerWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IUncheckedResultsService _resultsService;
    private readonly ITextComparer _comparer;

    public Worker(ILogger<Worker> logger, 
        IUncheckedResultsService results,
        ITextComparer comparer)
    {
        _logger = logger;
        _resultsService = results;
        _comparer = comparer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var results = await _resultsService.GetResultsWithNullUniqueness();
            foreach (var result in results)
            {
                var uniqueness = _comparer.Compare(result.SourceText, result.ResultText);
                await _resultsService.UpdateResultUniqueness(result.ResultUid, uniqueness);
            }
            await Task.Delay(20000, stoppingToken);
        }
    }
}