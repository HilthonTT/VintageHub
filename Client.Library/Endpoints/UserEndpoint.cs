using Client.Library.Endpoints.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class UserEndpoint : IUserEndpoint
{
    private const string ApiEndpointUrl = "api/User";
    private readonly HttpClient _httpClient;

    public UserEndpoint(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync(ApiEndpointUrl);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
            return users;
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
            using var response = await _httpClient.GetAsync($"{ApiEndpointUrl}/{id}");
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
