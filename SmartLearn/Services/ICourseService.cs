using SmartLearn.DTOs.CourseDTO;
using SmartLearn.Models;

namespace SmartLearn.Services
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CreateCourseDTO dto, int userId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
