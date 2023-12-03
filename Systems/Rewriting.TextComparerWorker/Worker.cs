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
            var results = await _resultsService.GetResultsWithNullUniquenessAsync(10);
            if (!results.Any())
            {
                _logger.LogInformation("No unchecked results found");
            }
            else
            {
                var uidList = AggregateUids(results);
                _logger.LogInformation("Processing results:\n\t{uidList}", uidList);
            
                results.AsParallel().ForAll(UpdateUniqueness);
            }
            await Task.Delay(20000, stoppingToken);
        }
    }

    private static string AggregateUids(IEnumerable<ResultCompareModel> results)
    {
        return results
            .Select(r => r.ResultUid.ToString())
            .Aggregate((a, b) => a + "\n\t" + b);
    }

    private async void UpdateUniqueness(ResultCompareModel model)
    {
        try
        {
            var similarity = _comparer.Compare(model.SourceText, model.ResultText);
            var uniqueness = 100 - similarity;
            await _resultsService.UpdateResultUniquenessAsync(model.ResultUid, 10);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
    
    
}