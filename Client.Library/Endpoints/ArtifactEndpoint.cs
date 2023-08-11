﻿using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class ArtifactEndpoint : IArtifactEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ArtifactEndpoint);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameVendorPrefix = $"{CacheName}_Vendor_";

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

            if (output is not null)
            {
                return output;
            }

            var response = await _httpClient.GetAsync("Artifact");
            response.EnsureSuccessStatusCode();

            output = await response.Content.ReadFromJsonAsync<List<ArtifactModel>>();
            await _localStorage.SetAsync(CacheName, output, CacheTimeSpan);

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
            string key = CacheNamePrefix + id;
            var output = await _localStorage.GetAsync<ArtifactModel>(key);

            if (output is not null)
            {
                return output;
            }

            var response = await _httpClient.GetAsync($"Artifact/{id}");
            response.EnsureSuccessStatusCode();

            output = await response.Content.ReadFromJsonAsync<ArtifactModel>();
            await _localStorage.SetAsync(key, output, CacheTimeSpan);

            return output;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task<ArtifactModel> GetArtifactByVendorIdAsync(int vendorId)
    {
        try
        {
            string key = CacheNameVendorPrefix + vendorId;
            var output = await _localStorage.GetAsync<ArtifactModel>(key);

            if (output is not null)
            {
                return output;
            }

            var response = await _httpClient.GetAsync($"Artifact/vendor/{vendorId}");
            response.EnsureSuccessStatusCode();

            output = await response.Content.ReadFromJsonAsync<ArtifactModel>();
            await _localStorage.SetAsync(key, output, CacheTimeSpan);

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
            var response = await _httpClient.PostAsJsonAsync("Artifact", artifact);
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
            var response = await _httpClient.PutAsJsonAsync("Artifact", artifact);
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
            var response = await _httpClient.DeleteAsync($"Artifact/{artifact.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
