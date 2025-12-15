using SmartLearn.DTOs.CourseDTO;
using SmartLearn.Models;

namespace SmartLearn.Services
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CreateCourseDTO dto, int userId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int userId);
        Task<Course?> UpdateCourseAsync(int courseId, UpdateCourseDTO dto, int userId);
        Task<bool> DeleteCourseAsync(int courseId, int userId);
    }
}
