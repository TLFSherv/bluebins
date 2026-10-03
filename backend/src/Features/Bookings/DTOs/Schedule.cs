using System.Text.Json.Serialization;

public record ScheduleRequest : IRequest<int>
{
    public int Id { get; set; } // the same as the booking id used to create the schedule
    public DateOnly Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency Frequency { get; set; }
    public bool MakeDefault { get; set; }
}

public record ScheduleView
{
    public DateOnly Date { get; set; }
    public Frequency? Frequency { get; set; }
    public bool IsDefault { get; set; }
}