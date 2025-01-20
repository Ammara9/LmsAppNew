﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.Validation;

namespace LMS.Shared.DTOs.CourseDTO
{
    public class CourseUpdateDto
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start Date is required")]
        [FutureDateAttribute(ErrorMessage = "Start Date must be a future date")]
        public DateTime StartDate { get; set; } 
    }
}
