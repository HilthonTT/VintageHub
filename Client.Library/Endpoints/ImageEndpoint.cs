using Client.Library.Endpoints.Interfaces;
using Client.Library.LocalStorage.Interfaces;
using Client.Library.Models.Cache;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Client.Library.Endpoints;
public class ImageEndpoint : IImageEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ImageEndpoint);
    private const string ApiEndpointUrl = "api/Image";
    private const int MaxAllowedSize = int.MaxValue;
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public ImageEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private static async Task<MultipartFormDataContent> CompressFileAsync(IBrowserFile imageFile)
    {
        using var imageStream = imageFile.OpenReadStream(MaxAllowedSize);

        using var image = await Image.LoadAsync(imageStream);

        var encoder = new JpegEncoder
        {
            Quality = 80,
        };

        using var compressedStream = new MemoryStream();
        await image.SaveAsync(compressedStream, encoder);

        compressedStream.Seek(0, SeekOrigin.Begin);

        var content = new MultipartFormDataContent
        {
            { new StreamContent(imageFile.OpenReadStream(MaxAllowedSize)), "imageFile", imageFile.Name }
        };

        return content;
    }

    public async Task<string> UploadImageAsync(IBrowserFile imageFile)
    {
        try
        {
            using var content = await CompressFileAsync(imageFile);

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
