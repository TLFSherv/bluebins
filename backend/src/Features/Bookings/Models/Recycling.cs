
public class Recycling : IEntity<int>
{
    public int Id { get; set; } // the same as BookingId
    public int NumberOfBags { get; set; } = 1;
    public ICollection<RecyclingItem>? RecyclingItems { get; set; }
    public Booking Booking { get; set; }
}
