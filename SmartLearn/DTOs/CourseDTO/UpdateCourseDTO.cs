namespace SmartLearn.DTOs.CourseDTO
{
    public class UpdateCourseDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
