namespace Server.Library.DataAccess;
public class UserData : IUserData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(UserData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;

    public UserData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spUser_{operation}";
    }

    private static object GetInsertParameters(UserModel user)
    {
        return new
        {
            user.ObjectIdentifier,
            user.FirstName,
            user.LastName,
            user.DisplayName,
            user.EmailAddress,
            user.Address
        };
    }

    private static object GetUpdateParameters(UserModel user)
    {
        return new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.DisplayName,
            user.EmailAddress,
            user.Address
        };
    }

    private void RemoveUserCache(UserModel user)
    {
        string idKey = CacheNamePrefix + user.Id;
        string oidKey = CacheNamePrefix + user.ObjectIdentifier;

        _cache.Remove(idKey);
        _cache.Remove(oidKey);
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var output = _cache.Get<List<UserModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<UserModel, dynamic>(storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<UserModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<UserModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<UserModel> GetUserByOidAsync(string oid)
    {
        string key = CacheNamePrefix + oid;
        var output = _cache.Get<UserModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByOid");
            object parameters = new { ObjectIdentifier = oid };

            output = await _sql.LoadFirstOrDefaultAsync<UserModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertUserAsync(UserModel user)
    {
        _cache.Remove(CacheName);

        string storedProcedure = GetStoredProcedure("Insert");
        object parameters = GetInsertParameters(user);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateUserAsync(UserModel user)
    {
        RemoveUserCache(user);

        string storedProcedure = GetStoredProcedure("Update");
        object parameters = GetUpdateParameters(user);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        RemoveUserCache(user);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { user.Id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
