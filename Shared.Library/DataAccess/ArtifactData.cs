namespace Shared.Library.DataAccess;
public class ArtifactData : IArtifactData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private readonly ISqlDataAccess _sql;
    private readonly IImageData _imageData;
    private readonly IMemoryCache _cache;
    private const string CacheName = nameof(ArtifactData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameVendorPrefix = $"{CacheName}_Vendor_";

    public ArtifactData(
        ISqlDataAccess sql,
        IImageData imageData,
        IMemoryCache cache)
    {
        _sql = sql;
        _imageData = imageData;
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

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsAsync()
    {
        var output = _cache.Get<List<ArtifactDisplayModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAllDetailed");
            var parameters = new DynamicParameters();

            var vendor = new VendorModel();
            var category = new CategoryModel();
            var era = new EraModel();

            output = await _sql.LoadDetailedDataAsync<ArtifactDisplayModel>(
                "Id", storedProcedure, parameters, vendor, category, era);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsByVendorIdAsync(int vendorId)
    {
        string key = CacheNameVendorPrefix + vendorId;
        var output = _cache.Get<List<ArtifactDisplayModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByVendorIdDetailed");
            var parameters = GetVendorIdParameters(vendorId);

            var vendor = new VendorModel();
            var category = new CategoryModel();
            var era = new EraModel();

            output = await _sql.LoadDetailedDataAsync<ArtifactDisplayModel>(
                "Id", storedProcedure, parameters, vendor, category, era);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<ArtifactDisplayModel> GetArtifactByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<ArtifactDisplayModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByIdDetailed");
            var parameters = ParameterHelper.GetIdParameters(id);

            var vendor = new VendorModel();
            var category = new CategoryModel();
            var era = new EraModel();

            output = await _sql.LoadFirstOrDefaultDetailedDataAsync<ArtifactDisplayModel>(
                "Id", storedProcedure, parameters, vendor, category, era);

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
        var artifact = await GetArtifactByIdAsync(id);
        RemoveArtifactCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        await _imageData.DeleteImageAsync(artifact.ImageId);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
