using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PlantillaComponents.Services;
using PruebaProyecto;
using PruebaProyecto.Handlers;
using PruebaProyecto.Services;
using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<PaginacionService>();
builder.Services.AddSingleton<ThemeService>();
builder.Services.AddScoped<TableEventEmitter>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenService>();
builder.Services.AddTransient<AuthRedirectHandler>();
builder.Services.AddTransient<AuthorizationHandler>();

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7044/api/v1/auth/");
});

builder.Services.AddHttpClient<UbicacionesService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7044/api/v1/ubicaciones/");
})
.AddHttpMessageHandler<AuthorizationHandler>()
.AddHttpMessageHandler<AuthRedirectHandler>();

await builder.Build().RunAsync();