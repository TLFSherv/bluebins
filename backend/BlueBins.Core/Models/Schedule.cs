
using System.Text.Json.Serialization;

public class Schedule
{
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency Frequency { get; private set; } = Frequency.Weekly;
    public DayOfWeek? DayOfWeek { get; private set; }
    public int? DayOfMonth { get; private set; }
    public Schedule(Frequency frequency, DayOfWeek? dayOfWeek, int? dayOfMonth)
    {
        if (frequency == Frequency.Monthly && dayOfMonth == null)
        {
            throw new InvalidDataException("DayOfMonth cannot be null if frequency is monthly.");
        }
        else if ((frequency == Frequency.Weekly ||
        frequency == Frequency.Biweekly) && dayOfWeek == null)
        {
            throw new InvalidDataException("DayOfWeek cannot be null if frequency is weekly or bi-weekly");
        }

        DayOfWeek = dayOfWeek;
        DayOfMonth = dayOfMonth;
        Frequency = frequency;
    }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Frequency
{
    Weekly = 0,
    Biweekly = 1,
    Monthly = 2
}
