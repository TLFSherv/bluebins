
using System.Text.Json.Serialization;

public class Schedule : IEntity<int>
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Frequency? Frequency { get; set; }
    public DateOnly CollectionDate => GetCollectionDate();
    private DateOnly GetCollectionDate()
    {
        if (Frequency is null || Frequency is global::Frequency.Once) return Date;
        DateOnly start = Date;
        DateOnly end = new DateOnly();
        int numOfDays = (int)Frequency.Value * 7;
        while (start <= end)
        {
            if (Frequency == global::Frequency.Monthly)
            {
                start.AddMonths(1);
                continue;
            }
            start.AddDays(numOfDays);
        }
        return start;
    }



}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Frequency
{
    Once = 0,
    Weekly = 1,
    Biweekly = 2,
    Triweekly = 3,
    Monthly = 4
}