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
        try
        {
            // Using the updated endpoint for course enrollments
            var response = await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>($"api/courseenrollment/{courseId}/students");

            if (response == null)
            {
                throw new Exception("No students found or error occurred.");
            }

            return response;
        }
        catch (Exception ex)
        {
            // Log the error here or show a user-friendly message
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task AssignStudentToCourse(int courseId, string studentId)
    {
        try
        {
            var assignStudentDto = new { StudentId = studentId };
            var response = await _httpClient.PostAsJsonAsync($"api/courseenrollment/{courseId}/assign-student", assignStudentDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to assign student to course.");
            }
        }
        catch (Exception ex)
        {
            // Handle errors (log or show to user)
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task UnassignStudentFromCourse(int courseId, string studentId)
    {
        try
        {
            var assignStudentDto = new { StudentId = studentId };
            var response = await _httpClient.PostAsJsonAsync($"api/courseenrollment/{courseId}/unassign-student", assignStudentDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to unassign student from course.");
            }
        }
        catch (Exception ex)
        {
            // Handle errors (log or show to user)
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}
