using System.ComponentModel.DataAnnotations;

public record AddScheduleRequest
{
    [Required]
    public DateOnly StartDate { get; set; }
    [Required]
    public FrequencyTypes Frequency { get; set; }
    public bool IsActive { get; set; } = true;
}

public record UpdateScheduleRequest : AddScheduleRequest
{
    [Required]
    public string? ScheduleId { get; set; }
}

public record ScheduleView : AddScheduleRequest;