using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Presemtation.Controllers;

[Route("api/courses")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly LmsContext _context;

    public CourseController(LmsContext context)
    {
        _context = context;
    }

    [HttpPost]
    //[Authorize]
    public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
    {
        if (courseDto == null)
        {
            return BadRequest("Course cannot be null.");
        }
        var course = new Course
        {
            Name = courseDto.Name,
            Description = courseDto.Description,
            StartDate = courseDto.StartDate,
        };

        _context.Courses!.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDemoAuth), new { id = course.Id }, course);
    }

    [HttpGet]
    public async Task<IActionResult> GetDemoAuth()
    {
        // Fetch data from the database
        var databaseCourses = await _context
            .Courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StartDate = c.StartDate,
            })
            .ToListAsync();

        // Add some hardcoded courses
        var hardcodedCourses = new List<CourseDto>
        {
            new CourseDto
            {
                Id = 1,
                Name = "C#",
                Description = "Introduction to C# and .NET",
                StartDate = DateTime.Now.AddDays(10),
            },
            new CourseDto
            {
                Id = 2,
                Name = "Python Basics",
                Description = "Learn Python from scratch",
                StartDate = DateTime.Now.AddDays(20),
            },
        };

        // Combine both lists
        var allCourses = databaseCourses.Concat(hardcodedCourses).ToList();

        return Ok(allCourses);
    }
}
