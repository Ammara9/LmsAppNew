using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDTO;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace LMS.Blazor.Client.Services
{

    public class CourseService : ICourseService
    {
        private readonly LmsContext _context;

        private readonly HttpClient _httpClient;

        public CourseService(HttpClient httpClient, LmsContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CourseDto>>("api/courses");
        }

        public async Task<List<ApplicationUser>> GetAssignedStudents(int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Enrollments)
                .Include(e => e.ApplicationUser)
                .Select(e => e.ApplicationUser)
                .ToListAsync();
        }

        public async Task AssignStudentToCourse(int courseId, string studentId)
        {
            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = studentId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task UnassignStudentFromCourse(int courseId, string studentId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
