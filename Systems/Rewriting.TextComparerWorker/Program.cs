using Rewriting.Context;
using Rewriting.Services.Contracts;
using Rewriting.Services.TextComparer;
using Rewriting.TextComparerWorker;
using Rewriting.TextComparerWorker.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddAppAutomapper();
        services.AddTextComparer();
        services.AddAppDbContextFactory();
        services.AddUncheckedResultsService();
        services.AddHostedService<Worker>();
    })
    .Build();

AppDbStateChecker.Check(host.Services);

host.Run();
