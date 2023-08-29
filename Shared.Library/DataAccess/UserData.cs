namespace Shared.Library.DataAccess;
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

    private static DynamicParameters GetOidParameters(string oid)
    {
        var parameters = new DynamicParameters();
        parameters.Add("ObjectIdentifier", oid);

        return parameters;
    }

    private static DynamicParameters GetInsertParameters(UserModel user)
    {
        var parameters = new DynamicParameters();
        parameters.Add("ObjectIdentifier", user.ObjectIdentifier);
        parameters.Add("FirstName", user.FirstName);
        parameters.Add("LastName", user.LastName);
        parameters.Add("DisplayName", user.DisplayName);
        parameters.Add("EmailAddress", user.EmailAddress);
        parameters.Add("Address", user.Address);

        return parameters;
    }

    private static DynamicParameters GetUpdateParameters(UserModel user)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id);
        parameters.Add("FirstName", user.FirstName);
        parameters.Add("LastName", user.LastName);
        parameters.Add("DisplayName", user.DisplayName);
        parameters.Add("EmailAddress", user.EmailAddress);
        parameters.Add("Address", user.Address);

        return parameters;
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
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<UserModel>(storedProcedure, parameters);

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
            var parameters = ParameterHelper.GetIdParameters(id);

            output = await _sql.LoadFirstOrDefaultAsync<UserModel>(storedProcedure, parameters);

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
            var parameters = GetOidParameters(oid);

            output = await _sql.LoadFirstOrDefaultAsync<UserModel>(storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertUserAsync(UserModel user)
    {
        _cache.Remove(CacheName);

        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParameters(user);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateUserAsync(UserModel user)
    {
        RemoveUserCache(user);

        string storedProcedure = GetStoredProcedure("Update");
        var parameters = GetUpdateParameters(user);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        RemoveUserCache(user);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
