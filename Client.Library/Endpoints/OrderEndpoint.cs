﻿using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class OrderEndpoint : IOrderEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(OrderEndpoint);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameSingle = $"{CacheName}_Single";
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }

    public async Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId)
    {
        try
        {
            string key = CacheNamePrefix + userId;
            var output = await _localStorage.GetAsync<List<OrderModel>>(key);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/user/{userId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                await _localStorage.SetAsync(key, output, CacheTimeSpan);
            }

            return output;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }

    public async Task<OrderModel> GetOrderByIdAsync(int id)
    {
        try
        {
            var cachedOrders = await _localStorage.GetAsync<List<OrderModel>>(CacheNameSingle);
            cachedOrders ??= new();

            var cachedOrder = cachedOrders.FirstOrDefault(o => o.Id == id);
            if (cachedOrder is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedOrder = await response.Content.ReadFromJsonAsync<OrderModel>();
                cachedOrders.Add(cachedOrder);
                await _localStorage.SetAsync(CacheNameSingle, cachedOrders, CacheTimeSpan);
            }

            return cachedOrder;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
