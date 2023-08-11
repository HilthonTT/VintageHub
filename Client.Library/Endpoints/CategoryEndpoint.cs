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
                var response = await _httpClient.GetAsync("Category");
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
            var response = await _httpClient.GetAsync($"Category/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryModel>();
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
            var response = await _httpClient.PostAsJsonAsync("Category", category);
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
            var response = await _httpClient.PutAsJsonAsync("Category", category);
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
            var response = await _httpClient.DeleteAsync($"Category/{category.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
