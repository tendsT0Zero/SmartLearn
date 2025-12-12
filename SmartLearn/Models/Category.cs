using System.ComponentModel.DataAnnotations;

namespace SmartLearn.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = null!;
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
