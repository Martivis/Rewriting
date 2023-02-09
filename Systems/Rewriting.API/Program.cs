using Rewriting.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddControllers();

services.AddAppSwagger();
services.AddAppVersioning();
services.AddAppHealthChecks();

var app = builder.Build();


app.UseAppSwagger();
app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
