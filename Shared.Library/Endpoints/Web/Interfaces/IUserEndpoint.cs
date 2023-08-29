﻿namespace Shared.Library.Endpoints.Web.Interfaces;
public interface IUserEndpoint
{
    Task DeleteUserAsync(UserModel user);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(int id);
    Task<UserModel> GetUserByOidAsync(string oid);
    Task<UserModel> InsertUserAsync(UserModel user);
    Task UpdateUserAsync(UserModel user);
}