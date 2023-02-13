using Rewriting.Context;
using Rewriting.Identity.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();


var services = builder.Services;

services.AddHttpContextAccessor();
services.AddAppDbContextFactory();
services.AddAppIdentityServer();
services.AddAppHealthCheck();

var app = builder.Build();
app.UseIdentityServer();
app.UseAppHealthChecks();

AppDbExistenceChecker.Check(app.Services);

app.Run();

