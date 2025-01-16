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
    [HttpGet]
    [Authorize]
    public IActionResult GetCourses()
    {
        return Ok(
            new CourseDto[]
            {
                new CourseDto { Id = 1, Name = "From course controller" },
                new CourseDto { Id = 2, Name = "Anka" },
                new CourseDto { Id = 3, Name = "Nisse" },
                new CourseDto { Id = 4, Name = "Pelle" },
            }
        );
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateCourses(CourseDto courseDto)
    {
        var returnCourse = new CourseDto { Id = courseDto.Id, Name = courseDto.Name };
        return Ok(returnCourse);
    }
}
