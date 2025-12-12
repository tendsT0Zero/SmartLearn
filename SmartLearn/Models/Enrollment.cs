using SmartLearn.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SmartLearn.Models
{
    public class Enrollment
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        [Required] public string PaymentStatus { get; set; } = SD.PaymentStatus.Pending.ToString();
        public int StudentId { get; set; }
        public User Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
