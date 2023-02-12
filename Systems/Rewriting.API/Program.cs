using Rewriting.API.Configuration;
using Rewriting.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddControllers();

services.AddAppDbContextFactory();

services.AddAppSwagger();
services.AddAppVersioning();
services.AddAppHealthChecks();

var app = builder.Build();


app.UseAppSwagger();
app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.MapControllers();

AppDbInitializer.Execute(app.Services);

app.Run();
