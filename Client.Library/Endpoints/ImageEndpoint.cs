using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models.Cache;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;

namespace Client.Library.Endpoints;
public class ImageEndpoint : IImageEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ImageEndpoint);
    private const string ApiEndpointUrl = "api/Image";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public ImageEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        try
        {
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(imageFile.OpenReadStream()), "image", imageFile.FileName }
            };

            using var response = await _httpClient.PostAsync(ApiEndpointUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return "";
        }
    }

    public async Task<byte[]> GetImageAsync(string objectId)
    {
        try
        {
            var cachedImages = await _localStorage.GetAsync<List<ImageModel>>(CacheName);
            cachedImages ??= new();

            var cachedImage = cachedImages.FirstOrDefault(image => image.ObjectIdentifier == objectId);

            if (cachedImage is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{objectId}");
                response.EnsureSuccessStatusCode();

                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                cachedImage = new ImageModel(objectId, imageBytes);
                cachedImages.Add(cachedImage);
                await _localStorage.SetAsync(CacheName, cachedImages, CacheTimeSpan);
            }

            return cachedImage.Data;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return null;
        }
    }

    public async Task DeleteImageAsync(string objectId)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{objectId}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
