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
        // Using the same base URI for the API call
        return await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"courses/{courseId}/students");
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        // Using the same base URI for the API call
        await _httpClient.PostAsJsonAsync($"courses/{courseId}/assign-student", new { StudentId = studentId });
    }

    public async Task UnassignStudentFromCourse(int courseId, string studentId)
    {
        // Using the same base URI for the API call
        await _httpClient.PostAsJsonAsync($"courses/{courseId}/unassign-student", new { StudentId = studentId });
    }
}
