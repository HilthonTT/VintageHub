using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class CategoryEndpoint : ICategoryEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(CategoryEndpoint);
    private const string CacheNameSingle = $"{CacheName}_Single";
    private const string ApiEndpointUrl = "api/Category";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public CategoryEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<CategoryModel>> GetAllCategoriesAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<CategoryModel>>(CacheName);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<CategoryModel>>();
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

    public async Task<CategoryModel> GetCategoryByIdAsync(int id)
    {
        try
        {
            var cachedCategories = await _localStorage.GetAsync<List<CategoryModel>>(CacheNameSingle);
            cachedCategories ??= new();

            var cachedCategory = cachedCategories.FirstOrDefault(c => c.Id == id);
            if (cachedCategory is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedCategory = await response.Content.ReadFromJsonAsync<CategoryModel>();
                cachedCategories.Add(cachedCategory);
                await _localStorage.SetAsync(CacheNameSingle, cachedCategory, CacheTimeSpan);
            }

            return cachedCategory;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<CategoryModel> InsertCategoryAsync(CategoryModel category)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, category);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task UpdateCategoryAsync(CategoryModel category)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, category);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteCategoryAsync(CategoryModel category)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{category.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
