using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

    [HttpGet]
    [Authorize]
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

    [HttpPost]
    
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
}
