using SmartLearn.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartLearn.DTOs.CategoryDTO
{
    public class CreateCategoryDTO
    {
        
        [Required] public string Name { get; set; } = null!;
        
    }
}
