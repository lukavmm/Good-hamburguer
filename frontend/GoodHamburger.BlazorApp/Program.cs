using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using GoodHamburger.BlazorApp;
using GoodHamburger.BlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Authorization
builder.Services.AddAuthorizationCore();

// LocalStorage Service
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

// AuthenticationStateProvider - Singleton para manter estado durante toda sessão
builder.Services.AddSingleton<AuthenticationStateProvider, SimpleAuthStateProvider>();
builder.Services.AddSingleton<SimpleAuthStateProvider>(); // Registrar também o tipo concreto

// Configure HttpClient BÁSICO (sem autenticação) - usado apenas para AuthService
builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:60059");
});

// Configure HttpClient AUTENTICADO (com Bearer token) - usado para outras chamadas
builder.Services.AddScoped<SimpleAuthorizationMessageHandler>();
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:60059");
})
.AddHttpMessageHandler<SimpleAuthorizationMessageHandler>();

// Add Services
builder.Services.AddSingleton<IAuthService, AuthServiceSimple>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMenuItemManagementService, MenuItemManagementService>();
builder.Services.AddSingleton<ToastService>();

var app = builder.Build();

// Inicializar autenticação do LocalStorage
var authService = app.Services.GetRequiredService<IAuthService>();
if (authService is AuthServiceSimple authServiceSimple)
{
    await authServiceSimple.InitializeAsync();
}

await app.RunAsync();



