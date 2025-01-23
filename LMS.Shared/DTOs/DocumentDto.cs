﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        //public int? ActivityId { get; set; }
    }
}
