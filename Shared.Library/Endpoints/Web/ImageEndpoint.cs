namespace Shared.Library.Endpoints.Web;
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

    public async Task<string> UploadImageAsync(IBrowserFile imageFile)
    {
        try
        {
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(imageFile.OpenReadStream(imageFile.Size)), "imageFile", imageFile.Name }
            };

            using var response = await _httpClient.PostAsync(ApiEndpointUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return "";
    }

    public string GetImage(string objectId)
    {
        try
        {
            string baseAddress = _httpClient.BaseAddress.ToString();
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return "https://dummyimage.com/600x400/000/fff";
            }

            return $"{baseAddress}{ApiEndpointUrl}/{objectId}";
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
