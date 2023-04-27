using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using Rewriting.Settings;
using Rewriting.WebApp;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMudServices();
services.AddHttpClient();
services.AddBlazoredLocalStorage();

services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
services.AddScoped<IAuthService, AuthService>();

services.AddScoped<IApiService, ApiService>();

services.AddScoped<NewOrdersService>();
services.AddScoped<UserOrdersService>();
services.AddScoped<OrderService>();

services.AddScoped<OrderOffersService>();
services.AddScoped<UserOffersService>();
services.AddScoped<OfferService>();

services.AddScoped<ContractResultsService>();
services.AddScoped<UserContractsService>();
services.AddScoped<ContractService>();

var settings = SettingsLoader.Load<WebAppSettings>("WebApp");
services.AddSingleton(settings);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
