using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using LMS.Shared.DTOs.DocumentDTO;
using Microsoft.AspNetCore.Authorization;
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
       /* public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(
            int courseId,
            int moduleId
        )
		{
			var documents = await _context
				.Documents!.Where(d => d.ModuleId == moduleId)
				.ToListAsync();

			return documents;
		} */

		// GET: api/courses/{courseId}/modules/{moduleId}/document/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<DocumentDto>> GetDocument(int courseId, int moduleId, int id)
		{
			var document = await _context.Documents!.FirstOrDefaultAsync(d =>
				d.Id == id && d.ModuleId == moduleId
			);

			if (document == null)
			{
				return NotFound();
			}
			var documentDto = new DocumentDto
			{
				Id = document.Id,
				Name = document.Name,
				Description = document.Description,
				FilePath = document.FilePath,
				UploadedAt = document.UploadedAt,
				CourseId = document.CourseId,
				ModuleId = document.ModuleId
			};
			return Ok(documentDto);
		}


        /* public async Task<ActionResult<DocumentDto>> PostDocument(
             int courseId,
             int moduleId,
             IFormFile file,
             [FromForm] string name,
             [FromForm] string description
         )*/

        // POST: api/courses/{courseId}/modules/{moduleId}/document
        [HttpPost]
        public async Task<ActionResult<DocumentDto>> PostDocument(int moduleId, [FromForm] DocumentAPIUploadDto documentDto)
        {
            // Check if the file is null or empty
            if (documentDto.File == null || documentDto.File.Length == 0)
            {
                return BadRequest("File is missing.");
            }

            // Check if the WebRootPath is valid
            if (string.IsNullOrEmpty(_environment.WebRootPath))
			{
				// Fallback path if WebRootPath is not configured
				_environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			}

			// Check if the file name is valid
			if (string.IsNullOrEmpty(documentDto.File.FileName))
			{
				return BadRequest("Invalid file name.");
			}

			// Build the file path and ensure all components are valid
			var uploadsDirectory = "uploads";
			var filePath = Path.Combine(_environment.WebRootPath, uploadsDirectory, documentDto.File.FileName);

			// Ensure the uploads directory exists, if not, create it
			var uploadsPath = Path.Combine(_environment.WebRootPath, uploadsDirectory);
			if (!Directory.Exists(uploadsPath))
			{
				Directory.CreateDirectory(uploadsPath);
			}

			// Save the file to the server
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await documentDto.File.CopyToAsync(stream);
			}

			// Create the document record
			var document = new Document
			{
				Name = documentDto.Name,
				Description = documentDto.Description,
				FilePath = $"/{uploadsDirectory}/{documentDto.File.FileName}",
				UploadedAt = documentDto.UploadedAt,
                ModuleId = documentDto.ModuleId
			};

			_context.Documents!.Add(document);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetDocument),
				new
				{
					//courseId,
					//moduleId,
					id = document.Id,
				},
				document
			);
		}
        /* 

         public async Task<ActionResult<DocumentDto>> PostDocument([FromBody] DocumentDto documentDto)
         {
             // Open the file stream
             // using var fileStream = new FileStream(documentDto.FilePath, FileMode.Open, FileAccess.Read);

             //IFormFile file = new FormFile(fileStream, 0, fileStream.Length, "uploadedFile", Path.GetFileName(documentDto.FilePath));
             // Check if the file is null or empty
             //if (file == null || file.Length == 0)
             //{
             //  return BadRequest("No file uploaded.");
             //}


             if (string.IsNullOrEmpty(documentDto.FilePath) || !System.IO.File.Exists(documentDto.FilePath))
             {
                 return BadRequest("Invalid file path.");
             }


             // Check if the WebRootPath is valid
             if (string.IsNullOrEmpty(_environment.WebRootPath))
             {
                 // Fallback path if WebRootPath is not configured
                 _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
             }

             // Check if the file name is valid
             if (string.IsNullOrEmpty(file.FileName))
             {
                 return BadRequest("Invalid file name.");
             }

             // Build the file path and ensure all components are valid
             var uploadsDirectory = "uploads";
             var filePath = Path.Combine(_environment.WebRootPath, uploadsDirectory, file.FileName);

             // Ensure the uploads directory exists, if not, create it
             var uploadsPath = Path.Combine(_environment.WebRootPath, uploadsDirectory);
             if (!Directory.Exists(uploadsPath))
             {
                 Directory.CreateDirectory(uploadsPath);
             }

             // Save the file to the server
             using (var stream = new FileStream(filePath, FileMode.Create))
             {
                 await file.CopyToAsync(stream);
             }

             // Create the document record
             var document = new Document
             {
                 Name = documentDto.Name,
                 Description = documentDto.Description,
                 FilePath = $"/{uploadsDirectory}/{file.FileName}",
                 UploadedAt = documentDto.UploadedAt,
                 ModuleId = documentDto.ModuleId

             };

             _context.Documents!.Add(document);
             await _context.SaveChangesAsync();

             return CreatedAtAction(
                 nameof(GetDocument),
                 new
                 {
                     //courseId,
                     //moduleId,
                     id = document.Id,
                 },
                 document
             );
         } */



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

    public class DocumentAPIUploadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? File { get; set; }
        public DateTime UploadedAt { get; set; }
        public int ModuleId { get; set; }
    }
}