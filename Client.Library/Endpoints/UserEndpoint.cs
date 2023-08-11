using Client.Library.Endpoints.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace Client.Library.Endpoints;
public class UserEndpoint : IUserEndpoint
{
    private readonly HttpClient _httpClient;

    public UserEndpoint(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Users");
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
            return users;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            throw;
        }
    }

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"User/{id}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return user;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            throw;
        }
    }

    public async Task<UserModel> GetUserByOidAsync(string oid)
    {
        try
        {
            var response = await _httpClient.GetAsync($"User/auth/{oid}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return user;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            throw;
        }
    }

    public async Task<UserModel> InsertUserAsync(UserModel user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("User", user);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadFromJsonAsync<UserModel>();
            return createdUser;
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            throw;
        }
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("User", user);
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }

    public async Task DeleteUserAsync(UserModel user)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"User/{user.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
        }
    }
}
