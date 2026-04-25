using GoodHamburger.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoodHamburger.BlazorApp.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<OrderModel> CreateOrderAsync(List<Guid> itemIds)
    {
        var request = new CreateOrderRequest { ItemIds = itemIds };
        var response = await _httpClient.PostAsJsonAsync("api/Orders", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderModel>() 
            ?? throw new Exception("Failed to create order");
    }

    public async Task<List<OrderModel>> GetAllOrdersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<OrderModel>>("api/Orders");
        return response ?? new List<OrderModel>();
    }

    public async Task<OrderModel> GetOrderByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<OrderModel>($"api/Orders/{id}") 
            ?? throw new Exception("Order not found");
    }

    public async Task<OrderModel> UpdateOrderAsync(Guid id, List<Guid> itemIds)
    {
        var request = new UpdateOrderRequest { ItemIds = itemIds };
        var response = await _httpClient.PutAsJsonAsync($"api/Orders/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<OrderModel>() 
            ?? throw new Exception("Failed to update order");
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/Orders/{id}");
        response.EnsureSuccessStatusCode();
    }
}
