using Rewriting.API.Configuration;
using Rewriting.Context;
using Rewriting.Services.UserAccount;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddControllers().AddValidator();

services.AddAppDbContextFactory();

services.AddAppSwagger();
services.AddAppVersioning();
services.AddAppHealthChecks();
services.AddAppAutomapper();
services.AddAppUserAccountService();

var app = builder.Build();


app.UseAppSwagger();
app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.MapControllers();

AppDbInitializer.Execute(app.Services);

app.Run();
