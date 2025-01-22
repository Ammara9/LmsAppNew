using Domain.Models.Entities;

namespace LMS.Shared.DTOs;

public class ActivityDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ActivityTypeId { get; set; }
    public int ModuleId { get; set; }
    public ActivityType? ActivityType { get; set; }
    public Module? Module { get; set; }
}