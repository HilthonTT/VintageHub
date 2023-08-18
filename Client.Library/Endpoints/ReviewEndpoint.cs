using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class ReviewEndpoint : IReviewEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ReviewEndpoint);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string ApiEndpointUrl = "api/Review";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public ReviewEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<ReviewModel>> GetAllReviewsByArtifactId(int artifactId)
    {
        try
        {
            string key = CacheNamePrefix + artifactId;
            var output = await _localStorage.GetAsync<List<ReviewModel>>(key);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/artifact/{artifactId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<ReviewModel>>();
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

    public async Task<ReviewModel> GetReviewByIdAsync(int id)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ReviewModel>();
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

    public async Task<ReviewModel> InsertReviewAsync(ReviewModel review)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, review);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ReviewModel>();
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

    public async Task UpdateReviewAsync(ReviewModel review)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, review);
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

    public async Task DeleteReviewAsync(ReviewModel review)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{review.Id}");
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
