using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public class ModuleRequestDto
    {
        public int Id { get; set; }
        public int? CourseId { get; set; } //Foreign key

        
    }
}
