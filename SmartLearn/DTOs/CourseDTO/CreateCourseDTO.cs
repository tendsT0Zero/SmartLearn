using SmartLearn.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartLearn.DTOs.CourseDTO
{
    public class CreateCourseDTO
    {
        [Required] public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Required] public decimal Price { get; set; }
        public IFormFile? CoverImage { get; set; }
        public int CategoryId { get; set; }

    }
}
