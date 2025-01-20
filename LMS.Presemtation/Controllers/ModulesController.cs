using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API
{
    [Route("api/courses/{courseId}/module")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsContext _context;

        public ModulesController(LmsContext context)
        {
            _context = context;
        }

        // GET: api/courses/{courseId}/module
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModules(int courseId)
        {
            var course = await _context
                .Courses!.Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound($"Course with ID {courseId} not found.");
            }

            var moduleDtos = course.Modules.Select(m => new ModuleDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                CourseId = m.CourseId,
            });

            return Ok(moduleDtos);
        }

        // GET: api/courses/{courseId}/module/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int courseId, int id)
        {
            var module = await _context.Modules!.FirstOrDefaultAsync(m =>
                m.Id == id && m.CourseId == courseId
            );

            if (module == null)
            {
                return NotFound($"Module with ID {id} for Course ID {courseId} not found.");
            }

            var moduleDto = new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                CourseId = module.CourseId,
            };

            return Ok(moduleDto);
        }

        // POST: api/courses/{courseId}/module
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostModule(int courseId, ModuleDto moduleDto)
        {
            var course = await _context.Courses!.FindAsync(courseId);

            if (course == null)
            {
                return NotFound($"Course with ID {courseId} not found.");
            }

            var module = new Module
            {
                Name = moduleDto.Name,
                Description = moduleDto.Description,
                StartDate = moduleDto.StartDate,
                EndDate = moduleDto.EndDate,
                CourseId = courseId,
            };

            _context.Modules!.Add(module);
            await _context.SaveChangesAsync();

            moduleDto.Id = module.Id;

            return CreatedAtAction(nameof(GetModule), new { courseId, id = module.Id }, moduleDto);
        }

        // DELETE: api/courses/{courseId}/module/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int courseId, int id)
        {
            var module = await _context.Modules!.FirstOrDefaultAsync(m =>
                m.Id == id && m.CourseId == courseId
            );

            if (module == null)
            {
                return NotFound($"Module with ID {id} for Course ID {courseId} not found.");
            }

            _context.Modules!.Remove(module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules!.Any(e => e.Id == id);
        }
    }
}
