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

    private void RemoveWishlistCache(int id)
    {
        string idKey = CacheNamePrefix + id;
        _cache.Remove(idKey);
    }

    public async Task<List<ArtifactModel>> GetAllArtifactsInWishlistAsync(int userId)
    {
        string key = CacheNamePrefix + userId;
        var output = _cache.Get<List<ArtifactModel>>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetArtifacts");
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<ArtifactModel>(storedProcedure, parameters);
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
            var parameters = new DynamicParameters();

            output = await _sql.LoadDataAsync<WishlistModel>(storedProcedure, parameters);
            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertWishlistAsync(WishlistModel wishlist)
    {
        RemoveWishlistCache(wishlist.Id);

        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParameters(wishlist);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> DeleteWishlistAsync(int id)
    {
        RemoveWishlistCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
