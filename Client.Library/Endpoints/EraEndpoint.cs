using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class EraEndpoint : IEraEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(EraEndpoint);
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public EraEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<EraModel>> GetAllErasAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<EraModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync("Era");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<EraModel>>();
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

    public async Task<EraModel> GetEraByIdAsync(int id)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"Era/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<EraModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<EraModel> InsertEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("Era", era);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<EraModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task UpdateEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync("Era", era);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"Era/{era.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
