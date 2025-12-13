using SmartLearn.Models;

namespace SmartLearn.Services
{
    public interface ICategoryService
    {
        Task<List<Category>?> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        Task<Category?> DeleteCategoryAsync(int id);
    }
}
