using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;

    public CourseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId)
    {
        var response = await _httpClient.GetAsync($"api/courseenrollment/{courseId}/students");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ApplicationUserDto>>();
        }
        return new List<ApplicationUserDto>();
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { StudentId = studentId }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"api/courseenrollment/{courseId}/assign-student", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to assign student to course.");
        }
    }

    public async Task UnassignStudentFromCourse(int courseId, string studentId)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { StudentId = studentId }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"api/courseenrollment/{courseId}/unassign-student", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to unassign student from course.");
        }
    }
}
