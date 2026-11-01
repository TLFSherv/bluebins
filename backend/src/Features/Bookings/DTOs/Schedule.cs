using System.Text.Json.Serialization;

public record ScheduleRequest : IRequest<int>
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency Frequency { get; set; } = Frequency.Once;
    public bool MakeDefault { get; set; }
}

public record ScheduleView
{
    public DateOnly Date { get; set; }
    public Frequency Frequency { get; set; } = Frequency.Once;
    public bool MakeDefault { get; set; }
    // Used by the frontend to know the type of the data object
    public string Type { get; } = "schedule";
}