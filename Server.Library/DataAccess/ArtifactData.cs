namespace Server.Library.DataAccess;
public class ArtifactData : IArtifactData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;
    private const string CacheName = nameof(ArtifactData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameVendorPrefix = $"{CacheName}_Vendor_";

    public ArtifactData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spArtifact_{operation}";
    }

    private static object GetInsertParameters(ArtifactModel artifact)
    {
        return new
        {
            artifact.Name,
            artifact.Description,
            artifact.ImageUrl,
            artifact.Quantity,
            artifact.Rating,
            artifact.Price,
            artifact.VendorId,
            artifact.CategoryId,
            artifact.EraId,
            artifact.Availability,
        };
    }

    private void RemoveArtifactCache(ArtifactModel artifact)
    {
        string idKey = CacheNamePrefix + artifact.Id;

        _cache.Remove(idKey);
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsAsync()
    {
        var output = _cache.Get<List<ArtifactModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<ArtifactModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsByVendorIdAsync(int vendorId)
    {
        string key = CacheNameVendorPrefix + vendorId;
        var output = _cache.Get<List<ArtifactModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByVendorId");
            object parameters = new { VendorId = vendorId };

            output = await _sql.LoadDataAsync<ArtifactModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<ArtifactModel> GetArtifactByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<ArtifactModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<ArtifactModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertArtifactAsync(ArtifactModel artifact)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        object parameters = GetInsertParameters(artifact);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateArtifactAsync(ArtifactModel artifact)
    {
        RemoveArtifactCache(artifact);

        string storedProcedure = GetStoredProcedure("Update");

        return await _sql.SaveDataAsync(storedProcedure, artifact);
    }

    public async Task<int> DeleteArtifactAsync(ArtifactModel artifact)
    {
        RemoveArtifactCache(artifact);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { artifact.Id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
