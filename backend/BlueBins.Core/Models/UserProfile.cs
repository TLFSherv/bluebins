public class UserProfile
{
    public required string Id { get; set; }
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
    public Location? DefaultLocation { get; set; }
    public Schedule? DefaultSchedule { get; set; }
    public ICollection<Booking>? Bookings { get; set; }

    // Calculated helper property (not mapped to a database column)
    public Booking? MostRecentBooking =>
        Bookings?.OrderByDescending(b => b.Schedule.CollectionDate).FirstOrDefault();
    public bool HasActiveBooking
    {
        get
        {
            if (Bookings == null) return false;
            return Bookings.Any(x => x.Status == BookingStatus.Scheduled);
        }
    }



}