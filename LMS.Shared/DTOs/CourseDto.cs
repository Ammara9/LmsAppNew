using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs;

public class CourseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Course Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start Date is required")]
    public DateTime StartDate { get; set; }
    public ICollection<Module> Modules { get; set; } = new List<Module>();
}
