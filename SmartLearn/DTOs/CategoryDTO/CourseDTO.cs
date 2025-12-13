using System.ComponentModel.DataAnnotations;

namespace SmartLearn.DTOs.CategoryDTO
{
    public class CourseDTO
    {
        
        [Required] public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Required] public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
