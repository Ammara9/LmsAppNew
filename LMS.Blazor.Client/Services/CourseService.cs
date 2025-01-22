using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using LMS.Shared.DTOs.CourseDTO;

public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;

    public CourseService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/assign-student", new { StudentId = studentId });
        response.EnsureSuccessStatusCode(); // Throws an exception if not successful
    }

    public async Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId)
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"api/courses/{courseId}/students");
    }

    public async Task UnassignStudentFromCourse(int courseId, AssignStudentDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/unassign-student", dto);
        response.EnsureSuccessStatusCode();
    }



}
