using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Presentation.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly LmsContext _context;

        public ActivitiesController(LmsContext context)
        {
            _context = context;
        }

        // GET: api/courses/{courseId}/modules/{moduleId}/activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivitiesDto>>> GetActivities(
            int courseId,
            int moduleId
        )
        {
            var module = await _context
                .Modules!.Include(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == courseId);

            if (module == null)
            {
                return NotFound($"Module with ID {moduleId} for Course ID {courseId} not found.");
            }

            var activityDtos = module.Activities.Select(a => new ActivitiesDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                ModuleId = a.ModuleId,
                CourseId = a.CourseId,
                ActivityType = a.ActivityType,
            });

            return Ok(activityDtos);
        }

        // GET: api/courses/{courseId}/modules/{moduleId}/activities/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivitiesDto>> GetActivity(
            int courseId,
            int moduleId,
            int id
        )
        {
            var activity = await _context.Activities!.FirstOrDefaultAsync(a =>
                a.Id == id && a.ModuleId == moduleId && a.CourseId == courseId
            );

            if (activity == null)
            {
                return NotFound(
                    $"Activity with ID {id} for Module ID {moduleId} and Course ID {courseId} not found."
                );
            }

            var activityDto = new ActivitiesDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                ModuleId = activity.ModuleId,
                CourseId = activity.CourseId,
                ActivityType = activity.ActivityType,
            };

            return Ok(activityDto);
        }

        // POST: api/courses/{courseId}/modules/{moduleId}/activities
        [HttpPost]
        public async Task<ActionResult<ActivitiesDto>> PostActivity(
            int courseId,
            int moduleId,
            ActivitiesDto activitiesDto
        )
        {
            var module = await _context.Modules!.FirstOrDefaultAsync(m =>
                m.Id == moduleId && m.CourseId == courseId
            );

            if (module == null)
            {
                return NotFound($"Module with ID {moduleId} for Course ID {courseId} not found.");
            }

            var activity = new Activity
            {
                Name = activitiesDto.Name,
                Description = activitiesDto.Description,
                StartDate = activitiesDto.StartDate,
                EndDate = activitiesDto.EndDate,
                ModuleId = moduleId,
                CourseId = courseId,
                ActivityType = activitiesDto.ActivityType,
            };

            _context.Activities!.Add(activity);
            await _context.SaveChangesAsync();

            activitiesDto.Id = activity.Id;

            return CreatedAtAction(
                nameof(GetActivity),
                new
                {
                    courseId,
                    moduleId,
                    id = activity.Id,
                },
                activitiesDto
            );
        }

        // DELETE: api/courses/{courseId}/modules/{moduleId}/activities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int courseId, int moduleId, int id)
        {
            var activity = await _context.Activities!.FirstOrDefaultAsync(a =>
                a.Id == id && a.ModuleId == moduleId && a.CourseId == courseId
            );

            if (activity == null)
            {
                return NotFound(
                    $"Activity with ID {id} for Module ID {moduleId} and Course ID {courseId} not found."
                );
            }

            _context.Activities!.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities!.Any(e => e.Id == id);
        }
    }
}
