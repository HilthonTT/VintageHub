namespace Server.Library.DataAccess;
public class CategoryData : ICategoryData
{
    private static readonly TimeSpan CacheTimeSpan = TimeSpan.FromMinutes(30);
    private const string CacheName = nameof(CategoryData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly ISqlDataAccess _sql;
    private readonly IMemoryCache _cache;

    public CategoryData(
        ISqlDataAccess sql,
        IMemoryCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static string GetStoredProcedure(string operation)
    {
        return $"dbo.spCategory_{operation}";
    }

    private static object GetInsertParams(CategoryModel category)
    {
        return new
        {
            category.Name,
            category.Description,
        };
    }

    private void RemoveCategoryCache(CategoryModel category)
    {
        string idKey = CacheNamePrefix + category.Id;
        _cache.Remove(idKey);
    }

    public async Task<List<CategoryModel>> GetAllCategoriesAsync()
    {
        var output = _cache.Get<List<CategoryModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            object parameters = new { };

            output = await _sql.LoadDataAsync<CategoryModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(CacheName, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<CategoryModel> GetCategoryByIdAsync(int id)
    {
        string key = CacheNamePrefix + id;
        var output = _cache.Get<CategoryModel>(key);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetById");
            object parameters = new { Id = id };

            output = await _sql.LoadFirstOrDefaultAsync<CategoryModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertCategoryAsync(CategoryModel category)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        object parameters = GetInsertParams(category);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateCategoryAsync(CategoryModel category)
    {
        RemoveCategoryCache(category);

        string storedProcedure = GetStoredProcedure("Update");

        return await _sql.SaveDataAsync(storedProcedure, category);
    }

    public async Task<int> DeleteCategoryAsync(CategoryModel category)
    {
        RemoveCategoryCache(category);

        string storedProcedure = GetStoredProcedure("Delete");
        object parameters = new { category.Id };

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
