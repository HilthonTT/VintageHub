namespace Shared.Library.DataAccess.Interfaces;
public interface ICategoryData
{
    Task<int> DeleteCategoryAsync(int id);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<int> InsertCategoryAsync(CategoryModel category);
    Task<int> UpdateCategoryAsync(CategoryModel category);
}