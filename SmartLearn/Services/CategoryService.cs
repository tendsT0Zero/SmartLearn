using Microsoft.EntityFrameworkCore;
using SmartLearn.Data;
using SmartLearn.Models;

namespace SmartLearn.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Category?> CreateCategoryAsync(Category category)
        {
            // checking if the same category already exists
            var anyCategory = await _context.Categories.FirstOrDefaultAsync(c=>c.Name.ToLower() == category.Name.ToLower());
            if(anyCategory != null)
            {
                return null;
            }
            await _context.Categories.AddAsync(category);
            await  _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(int id)
        {
            var anyCategory=await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);
            if(anyCategory == null)
            {
                return null;
            }
            _context.Categories.Remove(anyCategory);
            await _context.SaveChangesAsync();
            return anyCategory;
        }

        public async Task<List<Category>?> GetCategoriesAsync()
        {
            var categories=await _context.Categories.ToListAsync();
            if(categories == null || categories.Count == 0)
            {
                return null;
            }
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);
            if(category == null)
            {
                return null;
            }
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            var anyCategory=await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);
            if(anyCategory == null)
            {
                return null;
            }
            anyCategory.Name=category.Name;
            await _context.SaveChangesAsync();
            return anyCategory;
        }
    }
}
