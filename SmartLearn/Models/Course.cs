using System.ComponentModel.DataAnnotations;

namespace SmartLearn.Models
{
    public class Course
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Required] public decimal Price { get; set; }
        public string? ImageUrl { get; set; } 
        public int InstructorProfileId { get; set; }
        public InstructorProfile InstructorProfile { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
