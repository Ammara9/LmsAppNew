using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using System.Net.Http.Json;

public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;

    public CourseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId)
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"api/courses/{courseId}/students");
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/assign-student", new { StudentId = studentId });
    }

    public async Task UnassignStudentFromCourse(int courseId, string studentId)
    {
        await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/unassign-student", new { StudentId = studentId });
    }
}
