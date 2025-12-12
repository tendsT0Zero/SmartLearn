using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartLearn.Models
{
    public class User
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonIgnore] 
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } 

        public InstructorProfile? InstructorProfile { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}
