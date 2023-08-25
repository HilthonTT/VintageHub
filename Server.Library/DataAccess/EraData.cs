namespace Server.Library.DataAccess;
public class EraData : IEraData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(EraData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;

    public EraData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spEra_{operation}";
    }

    private static DynamicParameters GetInsertParams(EraModel era)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", era.Name);
        parameters.Add("Description", era.Description);

        return parameters;
    }

    private static DynamicParameters GetUpdateParams(EraModel era)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", era.Id);
        parameters.Add("Name", era.Name);
        parameters.Add("Description", era.Description);

        return parameters;
    }

    private void RemoveEraCache(int id)
    {
        string idKey = CacheNamePrefix + id;
        _cache.Remove(idKey);
    }

    public async Task<List<EraModel>> GetAllErasAsync()
    {
        var output = _cache.Get<List<EraModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<EraModel>(storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<EraModel> GetEraByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<EraModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            var parameters = ParameterHelper.GetIdParameters(id);

            output = await _sql.LoadFirstOrDefaultAsync<EraModel>(storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertEraAsync(EraModel era)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParams(era);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateEraAsync(EraModel era)
    {
        RemoveEraCache(era.Id);

        string storedProcedure = GetStoredProcedure("Update");
        var parameters = GetUpdateParams(era);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteEraAsync(int id)
    {
        RemoveEraCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
