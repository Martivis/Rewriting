using Rewriting.API;
using Rewriting.API.Configuration;
using Rewriting.Common.JsonConverters;
using Rewriting.Context;
using Rewriting.Services.UserAccount;
using Rewriting.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger();

var services = builder.Services;

var identitySettings = SettingsLoader.Load<IdentitySettings>("Identity");
services.AddSingleton(identitySettings);

services.AddHttpContextAccessor();
services.AddAppAuth(identitySettings);

services.AddControllers().AddValidator().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
});

services.AddAppDbContextFactory();
services.AddAppSwagger(identitySettings);
services.AddAppVersioning();
services.AddAppHealthChecks();
services.AddAppAutomapper();
services.AddAppUserAccountService();

var app = builder.Build();

app.UseAppAuth();

app.UseAppSwagger();
app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.MapControllers();

AppDbInitializer.Execute(app.Services);

app.Run();
