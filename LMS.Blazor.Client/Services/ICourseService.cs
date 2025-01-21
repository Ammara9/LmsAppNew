﻿using Domain.Models.Entities;
using LMS.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Blazor.Client.Services
{
    public interface ICourseService
    {
        Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId);
        Task AssignStudentToCourse(int courseId, string studentId);
        Task UnassignStudentFromCourse(int courseId, string studentId);
    }
}
