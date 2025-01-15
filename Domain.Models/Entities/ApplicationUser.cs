using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities;

//Separate ApplicationUser between projects
//Setup relationship with EF here!
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}