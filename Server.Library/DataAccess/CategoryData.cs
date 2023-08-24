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

    private static DynamicParameters GetInsertParams(CategoryModel category)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", category.Name);
        parameters.Add("Description", category.Description);
        
        return parameters;
    }

    private void RemoveCategoryCache(int id)
    {
        string idKey = CacheNamePrefix + id;
        _cache.Remove(idKey);
    }

    public async Task<List<CategoryModel>> GetAllCategoriesAsync()
    {
        var output = _cache.Get<List<CategoryModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = GetStoredProcedure("GetAll");
            var parameters = new DynamicParameters();

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
            var parameters = ParameterHelper.GetIdParameters(id);

            output = await _sql.LoadFirstOrDefaultAsync<CategoryModel, dynamic>(
                storedProcedure, parameters);

            _cache.Set(key, output, CacheTimeSpan);
        }

        return output;
    }

    public async Task<int> InsertCategoryAsync(CategoryModel category)
    {
        string storedProcedure = GetStoredProcedure("Insert");
        var parameters = GetInsertParams(category);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task<int> UpdateCategoryAsync(CategoryModel category)
    {
        RemoveCategoryCache(category.Id);

        string storedProcedure = GetStoredProcedure("Update");

        return await _sql.SaveDataAsync(storedProcedure, category);
    }

    public async Task<int> DeleteCategoryAsync(int id)
    {
        RemoveCategoryCache(id);

        string storedProcedure = GetStoredProcedure("Delete");
        var parameters = ParameterHelper.GetIdParameters(id);

        return await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
