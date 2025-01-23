using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.DocumentDTO
{
    public class DocumentUploadDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public IBrowserFile FilePath { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
    }
}
