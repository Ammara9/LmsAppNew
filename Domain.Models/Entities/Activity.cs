﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Foreign Keys
        public int ActivityTypeId { get; set; }
        public int ModuleId { get; set; }

        //Navigation Properties
        public ActivityType ActivityType { get; set; }
        public Module Module { get; set; }
    }
}
