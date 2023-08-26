namespace Client.Library.Endpoints;
public class WishlistEndpoint : IWishlistEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(WishlistEndpoint);
    private const string ApiEndpointUrl = "api/Wishlist";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public WishlistEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsInWishlistAsync(int userId)
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }

    public async Task<List<ArtifactModel>> InsertWishlistAsync(WishlistModel wishlist)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, wishlist);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ArtifactModel>>();
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

    public async Task DeleteWishlistasync(WishlistModel wishlist)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{wishlist.Id}");
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
