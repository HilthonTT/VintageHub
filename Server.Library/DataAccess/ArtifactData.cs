using Server.Library.Models.Display;

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

    private static DynamicParameters GetVendorIdParameters(int vendorId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("VendorId", vendorId);

        return parameters;
    }

    private static DynamicParameters GetInsertParameters(ArtifactModel artifact)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", artifact.Name);
        parameters.Add("Description", artifact.Description);
        parameters.Add("ImageId", artifact.ImageId);
        parameters.Add("Quantity", artifact.Quantity);
        parameters.Add("Rating", artifact.Rating);
        parameters.Add("DiscountAmount", artifact.DiscountAmount);
        parameters.Add("Price", artifact.Price);
        parameters.Add("VendorId", artifact.VendorId);
        parameters.Add("CategoryId", artifact.CategoryId);
        parameters.Add("EraId", artifact.EraId);
        parameters.Add("Availability", artifact.Availability);

        return parameters;
    }

    private void RemoveArtifactCache(int id)
    {
        string idKey = CacheNamePrefix + id;
        _cache.Remove(idKey);
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsAsync()
    {
        var output = _cache.Get<List<ArtifactModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<ArtifactModel>(storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsWithDetailsAsync()
    {
        string key = CacheName + "Details";
        var output = _cache.Get<List<ArtifactDisplayModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAllWithDetails");
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<ArtifactDisplayModel>(storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
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
            var parameters = GetVendorIdParameters(vendorId);

            output = await _sql.LoadDataAsync<ArtifactModel>(storedProcedure, parameters);

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
            var parameters = ParameterHelper.GetIdParameters(id);

            output = await _sql.LoadFirstOrDefaultAsync<ArtifactModel>(storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertArtifactAsync(ArtifactModel artifact)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParameters(artifact);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateArtifactAsync(ArtifactModel artifact)
    {
        RemoveArtifactCache(artifact.Id);

        string storedProcedure = GetStoredProcedure("Update");
        var parameters = ParameterHelper.GetArtifactUpdateParameters(artifact);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteArtifactAsync(int id)
    {
        RemoveArtifactCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
