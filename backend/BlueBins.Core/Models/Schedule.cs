
using System.Text.Json.Serialization;

public class Schedule
{
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency Frequency { get; set; }
    public DayOfWeek? DayOfWeek { get; set; }
    public int? DayOfMonth { get; set; }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Frequency
{
    Weekly = 0,
    Biweekly = 1,
    Monthly = 2
}
