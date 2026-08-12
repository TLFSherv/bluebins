using System.ComponentModel.DataAnnotations;

public record AddScheduleRequest
{
    public DateOnly StartDate { get; set; }
    public FrequencyTypes Frequency { get; set; }
    public bool IsActive { get; set; } = true;
}

public record ScheduleDTO : AddScheduleRequest
{
    public int? ScheduleId { get; set; }
    public bool SetAsDefault { get; set; }
}

public record UpdateScheduleRequest : AddScheduleRequest
{
    public int ScheduleId { get; set; }
}

public record ScheduleView : AddScheduleRequest;