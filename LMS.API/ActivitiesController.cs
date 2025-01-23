using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTO;
using Microsoft.AspNetCore.Authorization;

namespace LMS.API
{
    //[Route("api/modules/{moduleId}/activity")]
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly LmsContext _context;

        public ActivitiesController(LmsContext context)
        {
            _context = context;
        }

        // GET: api/activities/module/{moduleId}
        /// <summary>
        /// Retrieves all activities belonging to a specific module.
        /// </summary>
        /// <param name="moduleId">The ID of the module.</param>
        /// <returns>A list of activities for the specified module in the format of ActivityDto:s
        /// including the respective modules and courses they belong to.</returns>
        [HttpGet("module/{moduleId}")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities(int moduleId)
        {
            var activities = await _context.Activities
                .Include(a => a.ActivityType) // Include ActivityType
                .Include(a => a.Module)
                .ThenInclude(m => m.Course) // Include Course via Module
                .Where(a => a.ModuleId == moduleId) // Filter by moduleId
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    ActivityTypeId = a.ActivityTypeId,
                    ActivityTypeName = a.ActivityType.Name,
                    Module = new ModuleDto
                    {
                        Id = a.Module.Id,
                        Name = a.Module.Name,
                        Description = a.Module.Description,
                        StartDate = a.Module.StartDate,
                        EndDate = a.Module.EndDate,
                        CourseId = a.Module.CourseId,
                        Course = new CourseDto
                        {
                            Id = a.Module.Course.Id,
                            Name = a.Module.Course.Name,
                            Description = a.Module.Course.Description,
                            StartDate = a.Module.Course.StartDate
                        }
                    }
                })
                .ToListAsync();

            return Ok(activities);
        }


        //// GET: api/modules/{moduleId}/Activities/5
        //[HttpGet("{id}")]
        // GET: api/activities/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        //// PUT: api/modules/{moduleId}/Activities/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        // PUT: api/activities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, Activity activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/modules/{moduleId}/Activities
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        // POST: api/activities
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        //// DELETE: api/modules/{moduleId}/Activities/5
        //[HttpDelete("{id}")]
        // DELETE: api/activities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
