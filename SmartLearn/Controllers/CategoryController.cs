using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.DTOs.CategoryDTO;
using SmartLearn.Models;
using SmartLearn.Services;

namespace SmartLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            if (categories == null || categories.Count == 0)
            {
                return NotFound(new { Message = "No categories found." });
            }
            var categoryDTOs = new List<CategoryDTO>();
            foreach(var category in categories)
            {
                categoryDTOs.Add(new CategoryDTO
                {
                    Id=category.Id,
                    Name=category.Name
                });
            }
            return Ok(categoryDTOs);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
           var category= await _categoryService.GetCategoryByIdAsync(id);
            if(category == null)
            {
                return NotFound(new { Message = "Category not found." });
            }
            var categoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryDTO createCategoryDTO)
        {
            var categoryDomain = new Category
            {
                Name = createCategoryDTO.Name
            };
            var category=await _categoryService.CreateCategoryAsync(categoryDomain);
            if(category == null)
            {
                return BadRequest(new { Message = "Category creation failed. It might already exist." });
            }
            return Ok("Category created successfully.");
        }
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody]Category category)
        {
            var categoryDomain = new Category
            {
                Name = category.Name
            };
            var updatedCategory=await _categoryService.UpdateCategoryAsync(id,categoryDomain);
            if(updatedCategory == null)
            {
                return BadRequest(new { Message = "Category update failed. It might not exist." });
            }
            return Ok("Category updated successfully.");
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var deletedCategory=await _categoryService.DeleteCategoryAsync(id);
            if(deletedCategory == null)
            {
                return BadRequest(new { Message = "Category deletion failed. It might not exist." });
            }
            return Ok("Category deleted successfully.");
        }
    }
}
