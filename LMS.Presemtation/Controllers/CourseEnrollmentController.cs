﻿using Microsoft.AspNetCore.Mvc;
using LMS.Shared.DTOs;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseEnrollmentController : ControllerBase
    {
        private readonly LmsContext _context;

        public CourseEnrollmentController(LmsContext context)
        {
            _context = context;
        }

        // Get all students assigned to a specific course
        [HttpGet("{courseId}/students")]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetAssignedStudents(int courseId)
        {
            var students = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => new ApplicationUserDto
                {
                    Id = e.ApplicationUser.Id,
                    Name = e.ApplicationUser.Name,
                    Email = e.ApplicationUser.Email,
                    Role = e.ApplicationUser.Role
                })
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound("No students found for this course.");
            }

            return Ok(students);
        }

        // Assign a student to a course
        [HttpPost("{courseId}/assign-student")]
        public async Task<IActionResult> AssignStudentToCourse(int courseId, [FromBody] AssignStudentDto assignStudentDto)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var student = await _context.Users.FindAsync(assignStudentDto.StudentId);

            if (course == null || student == null)
            {
                return NotFound("Course or Student not found.");
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = assignStudentDto.StudentId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Unassign a student from a course
        [HttpPost("{courseId}/unassign-student")]
        public async Task<IActionResult> UnassignStudentFromCourse(int courseId, [FromBody] AssignStudentDto assignStudentDto)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == assignStudentDto.StudentId);

            if (enrollment == null)
            {
                return NotFound("The student is not assigned to this course.");
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class AssignStudentDto
    {
        public string StudentId { get; set; }
    }
}
