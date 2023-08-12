using Client.Library.Models;

namespace VintageHub.Client.Authentication.Interfaces;
public interface IUserDataVerifier
{
    Task<UserModel> LoadAndVerifyUserAsync();
}