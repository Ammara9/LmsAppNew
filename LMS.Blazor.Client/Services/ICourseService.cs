using Domain.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Blazor.Client.Services
{
    public interface ICourseService
    {
        Task<List<ApplicationUser>> GetAssignedStudents(int courseId);
        Task AssignStudentToCourse(int courseId, string studentId);
        Task UnassignStudentFromCourse(int courseId, string studentId);
    }
}
