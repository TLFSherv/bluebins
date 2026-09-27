public record ScheduleRequest : IRequest<int>
{
    public int Id { get; set; } // the same as the booking id used to create the schedule
    public DateOnly StartDate { get; set; }
    public Frequency Frequency { get; set; }
    public bool MakeDefault { get; set; }
}

public record ScheduleView
{
    public DateOnly StartDate { get; set; }
    public Frequency? Frequency { get; set; }
    public bool IsDefault { get; set; }
}