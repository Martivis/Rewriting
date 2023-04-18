using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Rewriting.Settings;
using Rewriting.WebApp;
using Rewriting.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMudServices();
services.AddHttpClient();
//services.AddAuthorizationCore();
services.AddBlazoredLocalStorage();

services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IOrderService, OrderService>();

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
