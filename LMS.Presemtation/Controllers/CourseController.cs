using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bogus.DataSets;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.CourseDTO;
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

    [HttpGet]// GET: Get All courses list (api/courses) 
    //[Authorize]
    public async Task<ActionResult> GetAllCourses()
    {
        var courses = await _context.Courses.ToListAsync();

        if (courses == null)
        {
            return NotFound(new { Message = $"Courses not found." });
        }

        var courseDtos = courses.Select(p => new CourseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            StartDate = p.StartDate
        });

        // Return all courses
        return Ok(courseDtos);
    }

    [HttpGet("{id}")] // GET: api/courses/5 - GetCourse by id
    
    public async Task<ActionResult> GetCourseByID(int id)
    {
        var course = await _context.Courses.Where(p => p.Id == id).FirstOrDefaultAsync();

        if (course == null)
        {
            return NotFound(new { Message = $"Course not found." });
        }

        var courseDtos = new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            StartDate = course.StartDate
        };

        // Return all courses
        return Ok(courseDtos);
    }

    [HttpPost]// Post: api/courses - Create new course

    public async Task<IActionResult> CreateCourses(CourseDto courseDto)
    {
        //var returnCourse = new CourseDto { Id = courseDto.Id, Name = courseDto.Name };
        //return Ok(returnCourse);

        if (courseDto == null)
        {
            return BadRequest("Course is null.");
        }

        var newCourse = new Course {
            Name = courseDto.Name,
            Description = courseDto.Description,
            StartDate = courseDto.StartDate
        };
        _context.Add(newCourse);
        await _context.SaveChangesAsync();
        return Ok(newCourse);
    }

    //PUT: api/courses/5 - Update course
    [HttpPut("{id}")]
    public async Task<IActionResult> EditCourse(int id, [FromBody] CourseUpdateDto updateCourseDto)
    {

        if (id != updateCourseDto.CourseId)
        {
            return BadRequest("Mismatched Course ID.");
        }

        try
        {
            
            var existingCourse = await _context.Courses.FindAsync(updateCourseDto.CourseId);
            if (existingCourse == null)
            {
                return NotFound("Course not found.");
            }
            

            existingCourse.Name = updateCourseDto.Name;
            existingCourse.Description = updateCourseDto.Description;
            existingCourse.StartDate = updateCourseDto.StartDate;

            await _context.SaveChangesAsync();

            return Ok(existingCourse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("delete/{id}")]
    
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleteCourse = await _context.Courses.FindAsync(id);
        if (deleteCourse != null)
        {
            _context.Courses.Remove(deleteCourse);
        }

        await _context.SaveChangesAsync();
        return Ok(deleteCourse);
    }
   
}
