namespace Server.Library.DataAccess;
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

    private static object GetInsertParams(VendorModel vendor)
    {
        return new
        {
            vendor.OwnerUserId,
            vendor.Name,
            vendor.Description,
            vendor.DateFounded,
        };
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

    public async Task<List<VendorModel>> GetAllVendorsAsync()
    {
        var output = _cache.Get<List<VendorModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<VendorModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<VendorModel> GetVendorByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<VendorModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<VendorModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<VendorModel>> GetAllVendorByOwnerUserIdAsync(int ownerUserId)
    {
        string key = CacheNameUserPrefix + ownerUserId;
        var output = _cache.Get<List<VendorModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByOwnerId");
            object parameters = new { OwnerUserId = ownerUserId };

            output = await _sql.LoadDataAsync<VendorModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertVendorAsync(VendorModel vendor)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        object parameters = GetInsertParams(vendor);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateVendorAsync(VendorModel vendor)
    {
        RemoveVendorCache(vendor.Id, vendor);

        string storedProcedure = GetStoredProcedure("Update");

        return await _sql.SaveDataAsync(storedProcedure, vendor);
    }

    public async Task<int> DeleteVendorAsync(int id)
    {
        RemoveVendorCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { Id = id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
