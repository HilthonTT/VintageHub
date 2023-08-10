using VintageHub.Server.Library.Models;

namespace VintageHub.Server.Library.DataAccess.Interfaces;
public interface ICategoryData
{
    Task<int> DeleteCategoryAsync(CategoryModel category);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<int> InsertCategoryAsync(CategoryModel category);
    Task<int> UpdateCategoryAsync(CategoryModel category);
}