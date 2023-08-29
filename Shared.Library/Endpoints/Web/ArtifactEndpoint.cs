namespace Shared.Library.Endpoints.Web;
public class ArtifactEndpoint : IArtifactEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(ArtifactEndpoint);
    private const string CacheNameSingle = $"{CacheName}_Single";
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

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<ArtifactDisplayModel>>(CacheName);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
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

    public async Task<ArtifactDisplayModel> GetArtifactByIdAsync(int id)
    {
        try
        {
            var cachedArtifacts = await _localStorage.GetAsync<List<ArtifactDisplayModel>>(CacheNameSingle);
            cachedArtifacts ??= new();

            var cachedArtifact = cachedArtifacts.FirstOrDefault(a => a.Id == id);
            if (cachedArtifact is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedArtifact = await response.Content.ReadFromJsonAsync<ArtifactDisplayModel>();

                cachedArtifacts.Add(cachedArtifact);
                await _localStorage.SetAsync(CacheNameSingle, cachedArtifacts, CacheTimeSpan);
            }

            return cachedArtifact;
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

    public async Task<List<ArtifactDisplayModel>> GetArtifactByVendorIdAsync(int vendorId)
    {
        try
        {
            string key = CacheNameVendorPrefix + vendorId;
            var output = await _localStorage.GetAsync<List<ArtifactDisplayModel>>(key);

            if (output is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/vendor/{vendorId}");
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<ArtifactDisplayModel>>();
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

    public async Task<ArtifactDisplayModel> InsertArtifactAsync(ArtifactModel artifact)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, artifact);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ArtifactDisplayModel>();
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
