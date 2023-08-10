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

    private static object GetInsertParams(EraModel era)
    {
        return new
        {
            era.Name,
            era.Description,
        };
    }

    private void RemoveEraCache(EraModel era)
    {
        string idKey = CacheNamePrefix + era.Id;
        _cache.Remove(idKey);
    }

    public async Task<List<EraModel>> GetAllErasAsync()
    {
        var output = _cache.Get<List<EraModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<EraModel, dynamic>(
                storedProcedure, parameters);

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
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<EraModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertEraAsync(EraModel era)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        object parameters = GetInsertParams(era);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateEraAsync(EraModel era)
    {
        RemoveEraCache(era);

        string storedProcedure = GetStoredProcedure("Update");

        return await _sql.SaveDataAsync(storedProcedure, era);
    }

    public async Task<int> DeleteEraAsync(EraModel era)
    {
        RemoveEraCache(era);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { era.Id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
