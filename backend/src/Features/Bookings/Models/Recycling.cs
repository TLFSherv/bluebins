
public class Recycling : IEntity<int>
{
    public int Id { get; set; } // the same as BookingId
    public int NumberOfBags { get; set; }
    public ICollection<RecyclingItem>? RecyclingItems { get; set; }
    public Booking Booking { get; set; }
}
