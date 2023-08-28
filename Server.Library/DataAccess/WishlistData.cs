namespace Server.Library.DataAccess;
public class WishlistData : IWishlistData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;
    private const string CacheName = nameof(WishlistData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private const string CacheNameUserPrefix = $"{CacheName}_User_";

    public WishlistData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spWishlist_{operation}";
    }

    private static DynamicParameters GetInsertParameters(WishlistModel wishlist)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", wishlist.UserId);
        parameters.Add("ArtifactId", wishlist.ArtifactId);

        return parameters;
    }

    private static DynamicParameters GetUserIdParameters(int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        return parameters;
    }

    private void RemoveWishlistCache(int userId)
    {
        string idKey = CacheNamePrefix + userId;
        _cache.Remove(idKey);

        idKey = CacheNameUserPrefix + userId;
        _cache.Remove(idKey);
    }

    private async Task<WishlistModel> GetWishlistByIdAsync(int id)
    {
        string storedProcedure = GetStoredProcedure("GetById");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.LoadFirstOrDefaultAsync<WishlistModel>(storedProcedure, parameters);
    }

    public async Task<List<ArtifactDisplayModel>> GetAllArtifactsInWishlistAsync(int userId)
    {
        string key = CacheNamePrefix + userId;
        var output = _cache.Get<List<ArtifactDisplayModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetArtifacts");
            var parameters = GetUserIdParameters(userId);

            var vendor = new VendorModel();
            var category = new CategoryModel();
            var era = new EraModel();

            output = await _sql.LoadDetailedDataAsync<ArtifactDisplayModel>(
                "Id", storedProcedure, parameters, vendor, category, era);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<List<WishlistModel>> GetAllWishlistsByUserIdAsync(int userId)
    {
        string key = CacheNameUserPrefix + userId;
        var output = _cache.Get<List<WishlistModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetByUserId");
            var parameters = GetUserIdParameters(userId);

            output = await _sql.LoadDataAsync<WishlistModel>(storedProcedure, parameters);
            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertWishlistAsync(WishlistModel wishlist)
    {
        RemoveWishlistCache(wishlist.UserId);

        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParameters(wishlist);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteWishlistAsync(int id)
    {
        var wishlist = await GetWishlistByIdAsync(id);
        RemoveWishlistCache(wishlist.UserId);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
