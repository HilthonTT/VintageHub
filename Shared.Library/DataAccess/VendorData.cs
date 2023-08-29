namespace Shared.Library.DataAccess;
public class VendorData : IVendorData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(VendorData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameUserPrefix = $"{CacheName}_User_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;

    public VendorData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spVendor_{operation}";
    }

    private static DynamicParameters GetOwnerIdParamters(int ownerUserId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("OwnerUserId", ownerUserId);

        return parameters;
    }

    private static DynamicParameters GetInsertParams(VendorModel vendor)
    {
        var parameters = new DynamicParameters();
        parameters.Add("OwnerUserId", vendor.OwnerUserId);
        parameters.Add("Name", vendor.Name);
        parameters.Add("Description", vendor.Description);
        parameters.Add("ImageId", vendor.ImageId);
        parameters.Add("DateFounded", vendor.DateFounded);

        return parameters;
    }

    private void RemoveVendorCache(int id, VendorModel vendor = null)
    {
        string idKey = CacheNamePrefix + id;
        _cache.Remove(idKey);

        if (vendor is not null)
        {
            string ownerKey = CacheNameUserPrefix + vendor.OwnerUserId;
            _cache.Remove(ownerKey);
        }
    }

    public async Task<List<VendorDisplayModel>> GetAllVendorsAsync()
    {
        var output = _cache.Get<List<VendorDisplayModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAllDetailed");
            var parameters = new DynamicParameters();

            var user = new UserModel();
            output = await _sql.LoadDetailedDataAsync<VendorDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<VendorDisplayModel> GetVendorByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<VendorDisplayModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByIdDetailed");
            var parameters = ParameterHelper.GetIdParameters(id);

            var user = new UserModel();
            output = await _sql.LoadFirstOrDefaultDetailedDataAsync<VendorDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<VendorDisplayModel>> GetAllVendorByOwnerUserIdAsync(int ownerUserId)
    {
        string key = CacheNameUserPrefix + ownerUserId;
        var output = _cache.Get<List<VendorDisplayModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByOwnerIdDetailed");
            var parameters = GetOwnerIdParamters(ownerUserId);

            var user = new UserModel();
            output = await _sql.LoadDetailedDataAsync<VendorDisplayModel>(
                "Id", storedProcedure, parameters, user);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertVendorAsync(VendorModel vendor)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParams(vendor);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateVendorAsync(VendorModel vendor)
    {
        RemoveVendorCache(vendor.Id, vendor);
        string storedProcedure = GetStoredProcedure("Update");
        var parameters = ParameterHelper.GetVendorUpdateParameters(vendor);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteVendorAsync(int id)
    {
        RemoveVendorCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
