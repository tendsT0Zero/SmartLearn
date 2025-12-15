using Microsoft.EntityFrameworkCore;
using SmartLearn.Data;
using SmartLearn.DTOs.CourseDTO;
using SmartLearn.Models;

namespace SmartLearn.Services
{
    public class CourseService : ICourseService
    {
 
            private readonly AppDbContext _context;
            private readonly IWebHostEnvironment _environment; 

            public CourseService(AppDbContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;
            }

            public async Task<Course> CreateCourseAsync(CreateCourseDTO dto, int userId)
            {
               var instructorProfile=await _context.InstructorProfiles.FirstOrDefaultAsync(ip => ip.UserId == userId);
                if (instructorProfile == null)
                {
                     throw new Exception("Instructor profile not found for the given user ID.");
                }
                int instructorId = instructorProfile.Id;
            string imageUrl = "default.jpg";

                if (dto.CoverImage != null)
                {
                    // Create "wwwroot/images/courses" folder if it doesn't exist
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "courses");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.CoverImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                   
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.CoverImage.CopyToAsync(fileStream);
                    }

                    
                    imageUrl = $"/images/courses/{uniqueFileName}";
                }

                //Creating Entity
                var course = new Course
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId,
                    InstructorProfileId = instructorId, 
                    ImageUrl = imageUrl 
                };


                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return course;
            }

            public async Task<IEnumerable<Course>> GetAllCoursesAsync()
            {
                return await _context.Courses
                             .Include(c => c.Category)
                             .Include(c => c.InstructorProfile)
                             .ToListAsync();
            }

            public async Task<Course?> GetCourseByIdAsync(int courseId)
            {
                return await _context.Courses
                    .Include(c => c.Category)
                    .Include(c => c.InstructorProfile)
                    .FirstOrDefaultAsync(c => c.Id == courseId);
            }

            public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int userId)
            {
                return await _context.Courses
                    .Include(c => c.Category)
                    .Include(c => c.InstructorProfile)
                    .Where(c => c.InstructorProfile.UserId == userId)
                    .ToListAsync();
            }

            public async Task<Course?> UpdateCourseAsync(int courseId, UpdateCourseDTO dto, int userId)
            {
                var course = await _context.Courses
                    .Include(c => c.InstructorProfile)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null) return null;

                if (course.InstructorProfile.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You do not own this course.");
                }

                course.Title = dto.Title;
                course.Description = dto.Description;
                course.Price = dto.Price;
                course.CategoryId = dto.CategoryId;

                if (dto.CoverImage != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "courses");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.CoverImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.CoverImage.CopyToAsync(fileStream);
                    }

                    course.ImageUrl = $"/images/courses/{uniqueFileName}";
                }

                await _context.SaveChangesAsync();
                return course;
            }

            public async Task<bool> DeleteCourseAsync(int courseId, int userId)
            {
                var course = await _context.Courses
                    .Include(c => c.InstructorProfile)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null) return false;

                if (course.InstructorProfile.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You do not own this course.");
                }

                if (!string.IsNullOrEmpty(course.ImageUrl) && course.ImageUrl != "default.jpg")
                {
                    var filePath = Path.Combine(_environment.WebRootPath, course.ImageUrl.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
    }
    
}
