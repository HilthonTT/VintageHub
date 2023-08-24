namespace VintageHub.Client.Authentication;

public class UserDataVerifier : IUserDataVerifier
{
    private readonly AuthenticationStateProvider _authState;
    private readonly IUserEndpoint _userEndpoint;

    public UserDataVerifier(AuthenticationStateProvider authState, IUserEndpoint userEndpoint)
    {
        _authState = authState;
        _userEndpoint = userEndpoint;
    }

    public async Task<UserModel> LoadAndVerifyUserAsync()
    {
        var authState = await _authState.GetAuthenticationStateAsync();
        string oid = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;

        if (string.IsNullOrWhiteSpace(oid))
        {
            return null;
        }

        var loggedInUser = await _userEndpoint.GetUserByOidAsync(oid) ?? new();

        string firstName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value;
        string lastName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("family_name"))?.Value;
        string displayName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        string email = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("emails"))?.Value;
        string streetAddress = authState.User.FindFirst("streetAddress").Value;

        string formattedEmail = RemoveBracketsAndQuotes(email);

        bool isDirty = false;

        if (oid.Equals(loggedInUser.ObjectIdentifier) is false)
        {
            isDirty = true;
            loggedInUser.ObjectIdentifier = oid;
        }

        if (firstName.Equals(loggedInUser.FirstName) is false)
        {
            isDirty = true;
            loggedInUser.FirstName = firstName;
        }

        if (lastName.Equals(loggedInUser.LastName) is false)
        {
            isDirty = true;
            loggedInUser.LastName = lastName;
        }

        if (displayName.Equals(loggedInUser.DisplayName) is false)
        {
            isDirty = true;
            loggedInUser.DisplayName = displayName;
        }

        if (formattedEmail.Equals(loggedInUser.EmailAddress) is false)
        {
            isDirty = true;
            loggedInUser.EmailAddress = formattedEmail;
        }

        if (streetAddress.Equals(loggedInUser.Address) is false)
        {
            isDirty = true;
            loggedInUser.Address = streetAddress;
        }

        if (isDirty)
        {
            if (loggedInUser.Id is 0 || loggedInUser.Id < 0)
            {
                return await _userEndpoint.InsertUserAsync(loggedInUser);
            }
            else
            {
                await _userEndpoint.UpdateUserAsync(loggedInUser);
                return loggedInUser;
            }
        }
        else
        {
            return loggedInUser;
        }
    }

    private static string RemoveBracketsAndQuotes(string input)
    {
        if (input.StartsWith("[") && input.EndsWith("]"))
        {
            input = input.Substring(1, input.Length - 2);
        }

        if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            input = input.Substring(1, input.Length - 2);
        }

        return input;
    }
}
