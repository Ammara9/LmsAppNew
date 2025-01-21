﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LMS.API
{
    [Route("api/courses/{courseId}/module/{moduleId}/document")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentsController(LmsContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/courses/{courseId}/modules/{moduleId}/document
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(
            int courseId,
            int moduleId
        )
        {
            var documents = await _context
                .Documents!.Where(d => d.ModuleId == moduleId)
                .ToListAsync();

            return documents;
        }

        // GET: api/courses/{courseId}/modules/{moduleId}/document/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int courseId, int moduleId, int id)
        {
            var document = await _context.Documents!.FirstOrDefaultAsync(d =>
                d.Id == id && d.ModuleId == moduleId
            );

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        // POST: api/courses/{courseId}/modules/{moduleId}/document
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(
            int courseId,
            int moduleId,
            IFormFile file,
            [FromForm] string name,
            [FromForm] string description
        )
        {
            // Check if the file is null or empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Save the file to the server
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Create the document record
            var document = new Document
            {
                Name = name,
                Description = description,
                FilePath = $"/uploads/{file.FileName}",
                UploadedAt = DateTime.UtcNow,
                ModuleId = moduleId,
            };

            _context.Documents!.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDocument),
                new
                {
                    courseId,
                    moduleId,
                    id = document.Id,
                },
                document
            );
        }

        // DELETE: api/courses/{courseId}/modules/{moduleId}/document/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int courseId, int moduleId, int id)
        {
            var document = await _context.Documents!.FirstOrDefaultAsync(d =>
                d.Id == id && d.ModuleId == moduleId
            );

            if (document == null)
            {
                return NotFound();
            }

            _context.Documents!.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
