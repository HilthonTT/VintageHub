using Client.Library.Endpoints.Interfaces;
using Client.Library.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace VintageHub.Client.Authentication;

public static class AuthenticationStateProviderHelpers
{
    public static async Task<UserModel> GetUserFromAuth(
        this AuthenticationStateProvider provider,
        IUserEndpoint userEndpoint)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string oid = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
        return await userEndpoint.GetUserByOidAsync(oid);
    }
}
