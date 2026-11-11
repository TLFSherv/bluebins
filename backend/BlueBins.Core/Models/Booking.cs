public class Booking
{
    public int Id { get; set; }
    public string UserProfileId { get; set; }
    public int LocationId { get; set; }
    public int? ScheduleId { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Draft;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public required DateOnly BookingDate { get; set; }
    public Location Location { get; private set; }
    public Schedule? Schedule { get; set; }
    public required Recycling Recycling { get; set; }
    private Booking() { }

    public Booking(Location location)
    {
        Location = location ?? throw new ArgumentNullException(nameof(location));
        DateCreated = DateTime.UtcNow;
    }

    // Changing the location requires a different booking date
    public void ChangeDateAndLocation(DateOnly bookingDate, Location location)
    {
        if (Status != BookingStatus.Scheduled && Status != BookingStatus.Draft)
        {
            throw new InvalidOperationException("You can't change the location of an inactive booking");
        }

        if (!string.Equals(Location.Parish, location.Parish, StringComparison.OrdinalIgnoreCase)
        && BookingDate.Equals(bookingDate))
        {
            throw new InvalidOperationException("A change of parish requires a new collection date");
        }

        Location = location;
        BookingDate = bookingDate;
        DateModified = DateTime.UtcNow;
    }

}

public enum BookingStatus
{
    Draft = 1,
    Scheduled = 2,
    InTransit = 3,
    Collected = 4,
    Contaminated = 5,
    Cancelled = 6
}