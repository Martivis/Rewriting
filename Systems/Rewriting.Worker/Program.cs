using Rewriting.API.Configuration;
using Rewriting.Services.RabbitMQ;
using Rewriting.Services.SmtpSender;
using Rewriting.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

services.AddHttpContextAccessor();
services.AddHealthChecks();
services.AddSmtpStub();
services.AddRabbitMQ();
services.AddSingleton<ITaskExecutor, TaskExecutor>();

var app = builder.Build();

app.UseAppHealthChecks();

var taskExecutor = app.Services.GetRequiredService<ITaskExecutor>();
taskExecutor!.Start();

app.Run();
