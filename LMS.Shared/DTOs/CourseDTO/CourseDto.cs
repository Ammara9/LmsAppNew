using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain.Models.Entities;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.CourseDTO;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }

    // utkommenterad för den användes inte och 
    //public ICollection<Module> Modules { get; set; } = new List<Module>();
}
