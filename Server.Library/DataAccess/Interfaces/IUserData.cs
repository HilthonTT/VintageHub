namespace Server.Library.DataAccess.Interfaces;
public interface IUserData
{
    Task<int> DeleteUserAsync(UserModel user);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(int id);
    Task<UserModel> GetUserByOidAsync(string oid);
    Task<int> InsertUserAsync(UserModel user);
    Task<int> UpdateUserAsync(UserModel user);
}