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
                using var response = await _httpClient.GetAsync("Vendor");
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
            using var response = await _httpClient.GetAsync($"Vendor/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<VendorModel>();
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
            using var response = await _httpClient.PostAsJsonAsync("Vendor", vendor);
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
            using var response = await _httpClient.PutAsJsonAsync("Vendor", vendor);
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
            using var response = await _httpClient.DeleteAsync($"Vendor/{vendor.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
