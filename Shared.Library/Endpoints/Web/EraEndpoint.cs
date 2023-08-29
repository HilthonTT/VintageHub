namespace Shared.Library.Endpoints.Web;
public class EraEndpoint : IEraEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(EraEndpoint);
    private const string CacheNameSingle = $"{CacheName}_Single";
    private const string ApiEndpointUrl = "api/Era";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public EraEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<EraModel>> GetAllErasAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<EraModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<EraModel>>();
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

    public async Task<EraModel> GetEraByIdAsync(int id)
    {
        try
        {
            var cachedEras = await _localStorage.GetAsync<List<EraModel>>(CacheNameSingle);
            cachedEras ??= new();

            var cachedEra = cachedEras.FirstOrDefault(e => e.Id == id);
            if (cachedEra is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedEra = await response.Content.ReadFromJsonAsync<EraModel>();
                cachedEras.Add(cachedEra);
                await _localStorage.SetAsync(CacheNameSingle, cachedEras, CacheTimeSpan);
            }

            return cachedEra;
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

    public async Task<EraModel> InsertEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, era);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<EraModel>();
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

    public async Task UpdateEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, era);
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

    public async Task DeleteEraAsync(EraModel era)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{era.Id}");
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
