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
    }
}
