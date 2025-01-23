using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CourseEnrollmentController : ControllerBase
{
    private readonly LmsContext _context;

    public CourseEnrollmentController(LmsContext context)
    {
        _context = context;
    }

    // Get all student-course mappings
    [HttpGet("enrollments")]
    public async Task<ActionResult<IEnumerable<(string StudentId, string CourseName, int CourseId)>>> GetStudentCourseMappings()
    {
        var mappings = await _context.Enrollments
            .Include(e => e.Course)
            .Select(e => new
            {
                e.StudentId,
                CourseName = e.Course.Name,
                e.CourseId
            })
            .ToListAsync();

        return Ok(mappings);
    }

    //// Get all students assigned to a specific course
    //[HttpGet("{courseId}/students")]
    //public async Task<ActionResult<List<ApplicationUserDto>>> GetAssignedStudents(int courseId)
    //{

    //    var students = await _context.Enrollments
    //        .Where(e => e.CourseId == courseId)
    //        .Select(e => new ApplicationUserDto
    //        {
    //            Id = e.ApplicationUser.Id,
    //            Name = e.ApplicationUser.Name,
    //            Email = e.ApplicationUser.Email,
    //            Role = e.ApplicationUser.Role
    //        })
    //        .ToListAsync();

    //    if (!students.Any())
    //    {
    //        return NotFound("No students found for this course.");
    //    }

    //    return Ok(students);
    //}

    // Assign a student to a course
    [HttpPost("{courseId}/assign-student")]
    public async Task<IActionResult> AssignStudentToCourse(int courseId, [FromBody] AssignStudentDto assignStudentDto)
    {
        // Validate course and student existence
        var course = await _context.Courses.FindAsync(courseId);
        var student = await _context.Users.FindAsync(assignStudentDto.StudentId);

        if (course == null || student == null)
        {
            return NotFound("Course or Student not found.");
        }

        // Check if the student is already enrolled in this course
        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == assignStudentDto.StudentId);

        if (existingEnrollment != null)
        {
            // Allow reassignment or simply log and return success
            Console.WriteLine($"Student {assignStudentDto.StudentId} is already assigned to Course {courseId}. No action taken.");
            return NoContent();
        }

        try
        {
            // Add new enrollment
            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = assignStudentDto.StudentId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return NoContent(); // Return success
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UC_StudentCourse") == true)
        {
            return Conflict("Student is already assigned to this course.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error assigning student: {ex.Message}");
            return StatusCode(500, "An error occurred while assigning the student. Please try again later.");
        }
    }

    // Unassign a student from a course
    [HttpPost("{courseId}/unassign-student")]
    public async Task<IActionResult> UnassignStudentFromCourse(int courseId, [FromBody] AssignStudentDto assignStudentDto)
    {
        if (assignStudentDto == null || string.IsNullOrEmpty(assignStudentDto.StudentId))
        {
            return BadRequest("Invalid request. StudentId cannot be null or empty.");
        }

        // Retrieve the enrollment record
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == assignStudentDto.StudentId);

        if (enrollment == null)
        {
            return NotFound("The student is not assigned to this course.");
        }

        try
        {
            // Remove the enrollment
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync(); // Persist the changes

            return NoContent(); // Return success
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error unassigning student: {ex.Message}");
            return StatusCode(500, "An error occurred while unassigning the student. Please try again later.");
        }
    }

}
