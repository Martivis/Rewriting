using Rewriting.API;
using Rewriting.API.Configuration;
using Rewriting.Context;
using Rewriting.Services.Cache;
using Rewriting.Services.Contracts;
using Rewriting.Services.EmailService;
using Rewriting.Services.Notifications;
using Rewriting.Services.Offers;
using Rewriting.Services.Orders;
using Rewriting.Services.RabbitMQ;
using Rewriting.Services.UserAccount;
using Rewriting.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

var identitySettings = SettingsLoader.Load<IdentitySettings>("Identity");
services.AddSingleton(identitySettings);

services.AddHttpContextAccessor();
services.AddAppAuth(identitySettings);

services.AddAppControllers();

services.AddAppDbContextFactory();
services.AddAppSwagger(identitySettings);
services.AddAppVersioning();
services.AddAppHealthChecks();
services.AddAppAutomapper();

services.AddAppUserAccountService();
services.AddOrderService();
services.AddOfferService();
services.AddContractService();

services.AddNotificationService();
services.AddEmailService();
services.AddRabbitMQ();
services.AddAppCache();

var app = builder.Build();

app.UseAppAuth();

app.UseAppSwagger();
app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.MapControllers();

app.UseAppMiddlewares();

AppDbInitializer.Execute(app.Services);
AppDbSeeder.Seed(app.Services);

app.Run();
