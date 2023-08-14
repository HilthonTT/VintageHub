using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class OrderEndpoint : IOrderEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(OrderEndpoint);
    private const string ApiEndpointUrl = "api/Order";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public OrderEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<OrderModel>> GetAllOrdersAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<OrderModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                await _localStorage.SetAsync(CacheName, output, CacheTimeSpan);
            }

            return output;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId)
    {
        try
        {
            var orders = await GetAllOrdersAsync();

            var cachedOrder = orders.Where(o => o.UserId == userId).ToList();

            if (cachedOrder?.Count <= 0)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/user/{userId}");
                response.EnsureSuccessStatusCode();

                cachedOrder = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
            }

            return cachedOrder;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<List<OrderDetailsModel>> GetOrderDetailsByOrderIdAsync(int orderId)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/orderDetails/{orderId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<OrderDetailsModel>>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<OrderModel> GetOrderByIdAsync(int id)
    {
        try
        {
            var orders = await GetAllOrdersAsync();

            var cachedOrder = orders.FirstOrDefault(o => o.Id == id);
            if (cachedOrder is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedOrder = await response.Content.ReadFromJsonAsync<OrderModel>();
            }

            return cachedOrder;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<OrderModel> InsertOrderAsync(OrderRequestModel request)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<OrderModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task UpdateOrderAsync(OrderRequestModel request)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, request);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteOrderAsync(OrderModel order)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{order.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
