using System.ComponentModel.DataAnnotations;

namespace SmartLearn.DTOs.CategoryDTO
{
    public class UpdateCategoryDTO
    {
        [Required] public string Name { get; set; } = null!;
    }
}
