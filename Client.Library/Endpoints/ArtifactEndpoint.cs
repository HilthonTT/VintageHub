using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class ArtifactEndpoint : IArtifactEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ArtifactEndpoint);
    private const string CacheNameVendorPrefix = $"{CacheName}_Vendor_";
    private const string ApiEndpointUrl = "api/Artifact";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public ArtifactEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<ArtifactModel>>(CacheName);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<ArtifactModel>>();
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

    public async Task<ArtifactModel> GetArtifactByIdAsync(int id)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ArtifactModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<List<ArtifactModel>> GetArtifactByVendorIdAsync(int vendorId)
    {
        try
        {
            string key = CacheNameVendorPrefix + vendorId;
            var output = await _localStorage.GetAsync<List<ArtifactModel>>(key);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/vendor/{vendorId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<ArtifactModel>>();
                await _localStorage.SetAsync(key, output, CacheTimeSpan);
            }

            return output;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task InsertArtifactAsync(ArtifactModel artifact)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, artifact);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task UpdateArtifactAsync(ArtifactModel artifact)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, artifact);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteArtifactAsync(ArtifactModel artifact)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{artifact.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
