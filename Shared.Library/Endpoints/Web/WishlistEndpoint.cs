namespace Shared.Library.Endpoints.Web;
public class WishlistEndpoint : IWishlistEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(WishlistEndpoint);
    private const string CacheNameUser = $"{CacheName}_User";
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

    private async Task RemoveCacheAsync()
    {
        await _localStorage.RemoveAsync(CacheName);
        await _localStorage.RemoveAsync(CacheNameUser);
    }

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsInWishlistAsync(int userId)
    {
        try
        {
            var output = await _localStorage.GetAsync<List<ArtifactDisplayModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/artifacts/{userId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<ArtifactDisplayModel>>();
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

    public async Task<List<WishlistModel>> GetAllWishlistsAsync(int userId)
    {
        try
        {
            var output = await _localStorage.GetAsync<List<WishlistModel>>(CacheNameUser);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{userId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<WishlistModel>>();
                await _localStorage.SetAsync(CacheNameUser, output, CacheTimeSpan);
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

    public async Task<List<ArtifactDisplayModel>> InsertWishlistAsync(WishlistModel wishlist)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, wishlist);
            response.EnsureSuccessStatusCode();

            await RemoveCacheAsync();
            return await response.Content.ReadFromJsonAsync<List<ArtifactDisplayModel>>();
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

            await RemoveCacheAsync();
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
