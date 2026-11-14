public class Booking
{
    public int Id { get; set; }
    public string UserProfileId { get; set; }
    public int LocationId { get; set; }
    public int? ScheduleId { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Draft;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public DateOnly BookingDate { get; set; }
    public Location Location { get; private set; }
    public Schedule? Schedule { get; set; }
    public required Recycling Recycling { get; set; }
    private Booking() { }

    public Booking(DateOnly bookingDate, Location location, Schedule? schedule)
    {
        BookingDate = bookingDate;
        Location = location ?? throw new ArgumentNullException(nameof(location));
        DateCreated = DateTime.UtcNow;

        /*
         In the future you can add the ability
         for the schedule start date to be different from booking date,
         and in that case, it's just a matter of replacing the booking state date
         with the schedule date. The Schedule table does not need to change.
         */
        if (schedule != null)
        {
            if (schedule.Frequency == Frequency.Monthly)
            {
                schedule.DayOfMonth = BookingDate.Day;
            }
            else
            {
                schedule.DayOfWeek = BookingDate.DayOfWeek;
            }
        }
        Schedule = schedule;
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