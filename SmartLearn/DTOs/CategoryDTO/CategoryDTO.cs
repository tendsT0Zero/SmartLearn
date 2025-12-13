using SmartLearn.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartLearn.DTOs.CategoryDTO
{
    public class CategoryDTO
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = null!;
    }
}
