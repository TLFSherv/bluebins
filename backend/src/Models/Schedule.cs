public class Schedule
{
    public int ScheduleId { get; set; }
    public DateOnly StartDate { get; set; }
    public FrequencyTypes Frequency { get; set; }
    public bool IsActive { get; set; }

}

public enum FrequencyTypes
{
    Weekly = 1,
    Fortnightly = 2,
    Monthly = 3
}