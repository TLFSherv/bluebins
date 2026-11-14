namespace Application.Dtos;

using System.Text.Json.Serialization;

public record ScheduleRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency Frequency { get; set; } = Frequency.Weekly;
    public bool MakeDefault { get; set; }
}

public record ScheduleView
{
    public Frequency Frequency { get; set; } = Frequency.Weekly;
    public bool MakeDefault { get; set; }
    // Used by the frontend to know the type of the data object
    public string Type { get; } = "schedule";
}