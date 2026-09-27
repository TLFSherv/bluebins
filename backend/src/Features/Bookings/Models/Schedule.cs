
public class Schedule : IEntity<int>
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public Frequency? Frequency { get; set; }
    public DateOnly CollectionDate => GetCollectionDate();
    private DateOnly GetCollectionDate()
    {
        if (Frequency is null) return StartDate;
        DateOnly start = StartDate;
        DateOnly end = new DateOnly();
        int numOfDays = 7;
        if (Frequency == global::Frequency.Biweekly)
            numOfDays = 14;
        else if (Frequency == global::Frequency.Triweekly)
            numOfDays = 21;
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

public enum Frequency
{
    Weekly = 1,
    Biweekly = 2,
    Triweekly = 3,
    Monthly = 4
}