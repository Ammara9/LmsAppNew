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
        _httpClient.BaseAddress = new Uri("https://localhost:7044/"); // Ensure this matches your API
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/assign-student", new { StudentId = studentId });
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error assigning student: {ex.Message}");
            throw;
        }
    }

    public async Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId)
    {
        try
        {
            var students = await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"api/courses/{courseId}/students");
            return students ?? new List<ApplicationUserDto>();
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error fetching assigned students: {ex.Message}");
            throw;
        }
    }

    public async Task UnassignStudentFromCourse(int courseId, AssignStudentDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/unassign-student", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error unassigning student: {ex.Message}");
            throw;
        }
    }
}

