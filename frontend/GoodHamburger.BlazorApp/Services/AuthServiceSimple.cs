using GoodHamburger.BlazorApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public class AuthServiceSimple : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILocalStorageService _localStorage;
    private string? _token;
    private string? _username;
    private string? _role;
    private bool _initialized = false;

    private const string TOKEN_KEY = "authToken";
    private const string USERNAME_KEY = "authUsername";
    private const string ROLE_KEY = "authRole";

    public AuthServiceSimple(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, ILocalStorageService localStorage)
    {
        _httpClientFactory = httpClientFactory;
        _serviceProvider = serviceProvider;
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;

        _token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
        _username = await _localStorage.GetItemAsync<string>(USERNAME_KEY);
        _role = await _localStorage.GetItemAsync<string>(ROLE_KEY);

        if (!string.IsNullOrEmpty(_token) && !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_role))
        {
            var authStateProvider = _serviceProvider.GetRequiredService<AuthenticationStateProvider>() as SimpleAuthStateProvider;
            authStateProvider?.NotifyUserAuthentication(_username, _role);
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("AuthClient");
        var response = await httpClient.PostAsJsonAsync("api/Auth/login", request);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? throw new Exception("Failed to login");
        
        _token = authResponse.Token;
        _username = authResponse.Username;
        _role = authResponse.Role;

        // Salvar no LocalStorage
        await _localStorage.SetItemAsync(TOKEN_KEY, _token);
        await _localStorage.SetItemAsync(USERNAME_KEY, _username);
        await _localStorage.SetItemAsync(ROLE_KEY, _role);

        // Resolve AuthenticationStateProvider do service provider
        var authStateProvider = _serviceProvider.GetRequiredService<AuthenticationStateProvider>() as SimpleAuthStateProvider;
        authStateProvider?.NotifyUserAuthentication(authResponse.Username, authResponse.Role);

        return authResponse;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("AuthClient");
        var response = await httpClient.PostAsJsonAsync("api/Auth/register", request);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? throw new Exception("Failed to register");

        return authResponse;
    }

    public async Task LogoutAsync()
    {
        _token = null;
        _username = null;
        _role = null;

        // Limpar LocalStorage
        await _localStorage.RemoveItemAsync(TOKEN_KEY);
        await _localStorage.RemoveItemAsync(USERNAME_KEY);
        await _localStorage.RemoveItemAsync(ROLE_KEY);

        // Resolve AuthenticationStateProvider do service provider - Precisei de auxilio de ia pra implementar isso, não sabia como resolver o AuthenticationStateProvider do service provider
        var authStateProvider = _serviceProvider.GetRequiredService<AuthenticationStateProvider>() as SimpleAuthStateProvider;
        authStateProvider?.NotifyUserLogout();
    }

    public Task<string?> GetTokenAsync()
    {
        return Task.FromResult(_token);
    }

    public Task<string?> GetUsernameAsync()
    {
        return Task.FromResult(_username);
    }

    public Task<string?> GetRoleAsync()
    {
        return Task.FromResult(_role);
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        return Task.FromResult(!string.IsNullOrEmpty(_token));
    }
}


