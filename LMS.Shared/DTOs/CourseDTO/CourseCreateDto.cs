using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.CourseDTO
{
    public class CourseCreateDto
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
    }
}
