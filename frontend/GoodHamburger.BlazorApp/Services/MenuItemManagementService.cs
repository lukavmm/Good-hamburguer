using GoodHamburger.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public class MenuItemManagementService : IMenuItemManagementService
{
    private readonly HttpClient _httpClient;

    public MenuItemManagementService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<MenuItemModel>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<MenuItemModel>>("api/MenuItems");
        return response ?? new List<MenuItemModel>();
    }

    public async Task<MenuItemModel> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<MenuItemModel>($"api/MenuItems/{id}")
            ?? throw new Exception("Menu item not found");
    }

    public async Task<MenuItemModel> CreateAsync(MenuItemModel item)
    {
        var response = await _httpClient.PostAsJsonAsync("api/MenuItems", item);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MenuItemModel>()
            ?? throw new Exception("Failed to create menu item");
    }

    public async Task<MenuItemModel> UpdateAsync(Guid id, MenuItemModel item)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/MenuItems/{id}", item);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MenuItemModel>()
            ?? throw new Exception("Failed to update menu item");
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/MenuItems/{id}");
        response.EnsureSuccessStatusCode();
    }
}
