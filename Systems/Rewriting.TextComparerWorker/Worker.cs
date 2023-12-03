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
            var results = await _resultsService.GetResultsWithNullUniquenessAsync(3);
            results.AsParallel().ForAll(UpdateUniqueness);
            await Task.Delay(2000, stoppingToken);
        }
    }

    private async void UpdateUniqueness(ResultCompareModel model)
    {
        var similarity = _comparer.Compare(model.SourceText, model.ResultText);
        var uniqueness = 100 - similarity;
        await _resultsService.UpdateResultUniquenessAsync(model.ResultUid, 10);
    }
}