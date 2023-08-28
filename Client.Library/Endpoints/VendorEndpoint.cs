namespace Client.Library.Endpoints;
public class VendorEndpoint : IVendorEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(VendorEndpoint);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameSingle = $"{CacheName}_Single";
    private const string ApiEndpointUrl = "api/Vendor";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public VendorEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<VendorDisplayModel>> GetAllVendorsAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<VendorDisplayModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<VendorDisplayModel>>();
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

    public async Task<VendorDisplayModel> GetVendorByIdAsync(int id)
    {
        try
        {
            var cachedVendors = await _localStorage.GetAsync<List<VendorDisplayModel>>(CacheNameSingle);
            cachedVendors ??= new();

            var cachedVendor = cachedVendors.FirstOrDefault(v => v.Id == id);
            if (cachedVendor is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedVendor = await response.Content.ReadFromJsonAsync<VendorDisplayModel>();
                cachedVendors.Add(cachedVendor);
                await _localStorage.SetAsync(CacheNameSingle, cachedVendors, CacheTimeSpan);
            }

            return cachedVendor;
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

    public async Task<List<VendorDisplayModel>> GetAllVendorsByOwnerUserIdAsync(int ownerUserId)
    {
        try
        {
            string key = CacheNamePrefix + ownerUserId;
            var vendors = await _localStorage.GetAsync<List<VendorDisplayModel>>(key);

            if (vendors?.Count <= 0)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/owner/{ownerUserId}");
                response.EnsureSuccessStatusCode();

                vendors = await response.Content.ReadFromJsonAsync<List<VendorDisplayModel>>();
                await _localStorage.SetAsync(key, vendors, CacheTimeSpan);
            }

            return vendors;
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

    public async Task<VendorDisplayModel> InsertVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, vendor);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<VendorDisplayModel>();
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

    public async Task UpdateVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, vendor);
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

    public async Task DeleteVendorAsync(VendorModel vendor)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{vendor.Id}");
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
