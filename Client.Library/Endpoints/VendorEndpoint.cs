using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class VendorEndpoint : IVendorEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(VendorEndpoint);
    private const string ApiEndpointUrl = "api/Vendor";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public VendorEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<VendorModel>> GetAllVendorsAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<VendorModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<VendorModel>>();
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

    public async Task<VendorModel> GetVendorByIdAsync(int id)
    {
        try
        {
            var vendors = await GetAllVendorsAsync();

            var cachedVendor = vendors.FirstOrDefault(v => v.Id == id);
            if (cachedVendor is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedVendor = await response.Content.ReadFromJsonAsync<VendorModel>();
            }

            return cachedVendor;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<List<VendorModel>> GetAllVendorsByOwnerUserIdAsync(int ownerUserId)
    {
        try
        {
            var vendors = await GetAllVendorsAsync();

            var cachedVendors = vendors.Where(v => v.OwnerUserId == ownerUserId).ToList();

            if (cachedVendors?.Count <= 0)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/owner/{ownerUserId}");
                response.EnsureSuccessStatusCode();

                cachedVendors = await response.Content.ReadFromJsonAsync<List<VendorModel>>();
            }

            return cachedVendors;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<VendorModel> InsertVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, vendor);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<VendorModel>();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task UpdateVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, vendor);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{vendor.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
