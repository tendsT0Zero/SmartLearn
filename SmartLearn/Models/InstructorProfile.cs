using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartLearn.Models
{
    public class InstructorProfile
    {
        [Key] public int Id { get; set; }
        public string? Bio { get; set; } = null;
        public string? PortfolioUrl { get; set; } = null;
        public int YearsOfExperience { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }
        [JsonIgnore]public User User { get; set; } = null!;
    }
}
