using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data;

public class LmsContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public LmsContext(DbContextOptions<LmsContext> options)
        : base(options) { }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Module> Modules { get; set; }
    public DbSet<Document>? Documents { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Enrollment relationships
        builder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>()
            .HasOne(e => e.ApplicationUser)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }


}
