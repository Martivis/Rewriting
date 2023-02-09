using Rewriting.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddControllers();

services.AddAppSwagger();
services.AddAppVersioning();

var app = builder.Build();


app.UseAppSwagger();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
