namespace Shared.Library.Endpoints.Web.Interfaces;
public interface ICategoryEndpoint
{
    Task DeleteCategoryAsync(CategoryModel category);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<CategoryModel> InsertCategoryAsync(CategoryModel category);
    Task UpdateCategoryAsync(CategoryModel category);
}