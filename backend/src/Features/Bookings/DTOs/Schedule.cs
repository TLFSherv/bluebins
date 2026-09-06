public record ScheduleRequest : IRequest<int>
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public FrequencyTypes Frequency { get; set; }
    public bool IsActive { get; set; } = true;
    public bool SetAsDefault { get; set; }
}

public record ScheduleView
{
    public DateOnly StartDate { get; set; }
    public FrequencyTypes Frequency { get; set; }
    public bool IsActive { get; set; } = true;
    public bool SetAsDefault { get; set; }
}