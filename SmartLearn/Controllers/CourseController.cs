using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.DTOs.CourseDTO;
using SmartLearn.Services;
using System.Security.Claims;

namespace SmartLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return int.Parse(userIdClaim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return Ok(courses);
        }
        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDTO dto)
        {
            // Fetching Current User's ID from the JWT Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Unauthorized user.");

            int instructorId = int.Parse(userIdClaim.Value);
            var course = await _courseService.CreateCourseAsync(dto, instructorId);

            return Ok(new { Message = "Course created!", CourseId = course.Id, ImageUrl = course.ImageUrl });
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute]int id, [FromForm] UpdateCourseDTO dto)
        {
            var userId = GetUserId();
            try
            {
                var updatedCourse = await _courseService.UpdateCourseAsync(id, dto, userId);

                if (updatedCourse == null)
                    return NotFound(new { Message = "Course not found." });

                return Ok(new { Message = "Course updated successfully.", Course = updatedCourse });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

      
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userId = GetUserId();
            try
            {
                var success = await _courseService.DeleteCourseAsync(id, userId);

                if (!success)
                    return NotFound(new { Message = "Course not found." });

                return Ok(new { Message = "Course deleted successfully." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourseById([FromRoute]int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { Message = "Course not found." });
            }
            return Ok(course);
        }

        [HttpGet("my-courses")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = GetUserId();
            var courses = await _courseService.GetCoursesByInstructorAsync(userId);
            return Ok(courses);
        }
    }
}
