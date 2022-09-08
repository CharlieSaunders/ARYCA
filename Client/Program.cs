using Blazored.LocalStorage;
using Client.Authentication;
using Client.States;
using Client.States.Toast;
using Common.HttpClients;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true);

builder.Services.AddScoped(serviceProvider => new GenericHttpClient("http://api-container:9999"));
builder.Services.AddScoped(serviceProvider => new BinanceHttpClient());
builder.Services.AddScoped(serviceProvider => new ToasterService());
builder.Services.AddScoped<UserState>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();