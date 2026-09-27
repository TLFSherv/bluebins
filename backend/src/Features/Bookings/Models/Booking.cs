public class Booking : IEntity<int>
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int LocationId { get; set; }
    public int ScheduleId { get; set; }
    public BookingStatus Status { get; private set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public UserProfile UserProfile { get; set; }
    public Location Location { get; private set; }
    public Schedule Schedule { get; set; }
    public Recycling Recycling { get; set; }

    private Booking()
    { }

    public Booking(UserProfile user, Recycling recycling, Location? location, Schedule? schedule)
    {
        // A user can only have one active booking
        if (user.MostRecentBooking != null &&
            user.MostRecentBooking.Status == BookingStatus.Scheduled)
        {
            throw new Exception("User can't have multiple active bookings");
        }

        var defaultLocation = UserProfile?.DefaultLocation;
        if (location is null && defaultLocation is null)
        {
            throw new Exception("A location is required");
        }

        var defaultSchedule = UserProfile?.DefaultSchedule;
        if (schedule is null && defaultSchedule is null)
        {
            throw new Exception("A schedule is required");
        }

        UserProfile = user;
        UserId = user.Id;
        // Override the users default location if location is provided
        Location = (location ?? defaultLocation)!;
        // Override the users default schedule if schedule is provided
        Schedule = (schedule ?? defaultSchedule)!;
        Recycling = recycling;
        Status = BookingStatus.Scheduled;
        DateCreated = DateTime.Today;
    }

    // Changing the location requires a different collection date
    public void SetLocation(Location location, Schedule schedule)
    {
        if (Status != BookingStatus.Scheduled)
        {
            throw new Exception("You can't change the location of a inactive booking");
        }

        if (!string.Equals(Location.Parish, location.Parish)
        && Schedule.CollectionDate.Equals(schedule.CollectionDate))
        {
            throw new Exception("A change of parish requires a new collection date");
        }

        Location = location;
        Schedule = schedule;
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