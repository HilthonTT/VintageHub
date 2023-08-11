using Client.Library.Models;

namespace Client.Library.Endpoints.Interfaces;
public interface ICategoryEndpoint
{
    Task DeleteCategoryAsync(CategoryModel category);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<CategoryModel> InsertCategoryAsync(CategoryModel category);
    Task UpdateCategoryAsync(CategoryModel category);
}