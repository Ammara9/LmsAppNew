using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities;

//Separate ApplicationUser between projects
//Setup relationship with EF here!
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
