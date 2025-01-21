﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public int CourseId { get; set; } // Foreign Key
        public Course? Course { get; set; } // Navigation property
    }
}
