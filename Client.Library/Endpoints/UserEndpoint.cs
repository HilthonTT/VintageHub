namespace Client.Library.Endpoints;
public class UserEndpoint : IUserEndpoint
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(UserEndpoint);
    private const string CacheNameSingle = $"{CacheName}_Single";
    private const string ApiEndpointUrl = "api/User";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorage _localStorage;

    public UserEndpoint(
        HttpClient httpClient,
        ILocalStorage localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        try
        {
            var output = await _localStorage.GetAsync<List<UserModel>>(CacheName);
            if (output is null)
            {
                using var response = await _httpClient.GetAsync(ApiEndpointUrl);
                response.EnsureSuccessStatusCode();

                output = await response.Content.ReadFromJsonAsync<List<UserModel>>();
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

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        try
        {
            var cachedUsers = await _localStorage.GetAsync<List<UserModel>>(CacheNameSingle);
            cachedUsers ??= new();

            var cachedUser = cachedUsers.FirstOrDefault(x => x.Id == id);

            if (cachedUser is null)
            {
                using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
                response.EnsureSuccessStatusCode();

                cachedUser = await response.Content.ReadFromJsonAsync<UserModel>();

                cachedUsers.Add(cachedUser);
                await _localStorage.SetAsync(CacheNameSingle, cachedUsers, CacheTimeSpan);
            }
      
            return cachedUser;
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

    public async Task<UserModel> GetUserByOidAsync(string oid)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/auth/{oid}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return user;
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

    public async Task<UserModel> InsertUserAsync(UserModel user)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(ApiEndpointUrl, user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserModel>();
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

    public async Task UpdateUserAsync(UserModel user)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(ApiEndpointUrl, user);
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

    public async Task DeleteUserAsync(UserModel user)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"{ApiEndpointUrl}/{user.Id}");
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
