using Bogus;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure.Data;

public static class SeedData
{
    private static UserManager<ApplicationUser> userManager = null!;
    private static RoleManager<IdentityRole> roleManager = null!;
    private const string adminRole = "Admin";

    public static async Task SeedDataAsync(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var db = serviceProvider.GetRequiredService<LmsContext>();

            if (await db.Users.AnyAsync())
                return;

            userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>()
                ?? throw new ArgumentNullException(nameof(userManager));
            roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>()
                ?? throw new ArgumentNullException(nameof(roleManager));

            var roleNames = new[] { "Admin", "Teacher", "Student" };

            try
            {
                await CreateRolesAsync(roleNames);

                var adminUser = await GenerateAdminAsync("Admin", "lms_admin@madeup.domain", "1Hemligt!", roleNames[0]);
                var teacherUser = await GenerateAdminAsync("Teacher", "lms_teacher@madeup.domain", "Pwteacher@11", roleNames[1]);
                var studentUser = await GenerateAdminAsync("Student", "lms_student@madeup.domain", "Pwstudent@22", roleNames[2]);

                await GenerateUsersAsync(5);

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    private static async Task CreateRolesAsync(string[] roleNames)
    {
        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;
            var role = new IdentityRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task GenerateUsersAsync(int nrOfUsers)
    {
        var faker = new Faker<ApplicationUser>("sv").Rules(
            (f, e) =>
            {
                e.Email = f.Person.Email;
                e.UserName = f.Person.Email;
            }
        );

        var users = faker.Generate(nrOfUsers);

        //ToDo: Add to user.secrets
        var passWord = "BytMig123!";
        if (string.IsNullOrEmpty(passWord))
            throw new Exception("password not found");

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task<ApplicationUser> GenerateAdminAsync(string userName, string emailId, string passWord, string roleName)
    {

        //ToDo: Add to user.secrets
        //var passWord = "1Hemligt!";
        var found = await userManager.FindByEmailAsync(emailId);

        if (found != null) return null!;

        if (string.IsNullOrEmpty(passWord))
            throw new Exception("password not found");

        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            Email = emailId,
            PasswordHash = passWord
        };

        var result = await userManager.CreateAsync(applicationUser, passWord);
        var result2 = await userManager.AddToRoleAsync(applicationUser, roleName);
        if (!result.Succeeded || !result2.Succeeded)
            throw new Exception(string.Join("\n", result.Errors, result2.Errors));

        return applicationUser;
    }

    private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
    {
        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

        }
    }
}
