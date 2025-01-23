using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using LMS.Shared.DTOs.CourseDTO;

public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;

    private readonly IApiService _apiService;


    public CourseService(HttpClient httpClient, IApiService apiService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri("https://localhost:7044/"); // Ensure this matches your API

        _apiService = apiService;
    }

    public async Task<List<ApplicationUserDto>> GetAssignedStudents(int courseId)
    {
        try
        {
            var students = await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"api/CourseEnrollment/{courseId}/students");
            return students ?? new List<ApplicationUserDto>();
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error fetching assigned students: {ex.Message}");
            throw;
        }
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/CourseEnrollment/{courseId}/assign-student", new { StudentId = studentId });

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"API returned error: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in AssignStudentToCourse: {ex.Message}");
            throw;
        }
    }

    public async Task UnassignStudentFromCourse(int courseId, string studentId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/CourseEnrollment/{courseId}/unassign-student", new { StudentId = studentId });
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error unassigning student: {ex.Message}");
            throw;
        }
    }

    public async Task<string> GetCourseNameById(int courseId)
    {
        try
        {
            var course = await _apiService.GetAsync<CourseDto>($"api/courses/{courseId}");
            return course?.Name ?? "Unknown Course"; // Fallback to "Unknown Course" if course not found
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching course name: {ex.Message}");
            return "Unknown Course";
        }
    }
}

