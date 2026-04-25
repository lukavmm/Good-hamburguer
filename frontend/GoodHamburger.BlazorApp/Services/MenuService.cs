using GoodHamburger.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public class MenuService : IMenuService
{
    private readonly HttpClient _httpClient;

    public MenuService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<MenuItemModel>> GetMenuAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<MenuItemModel>>("api/menu");
        return response ?? new List<MenuItemModel>();
    }
}
