using System.ComponentModel.DataAnnotations;

public class Booking
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int UserProfileId { get; set; }
    [Required]
    public int LocationId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
    [Required]
    public BookingStatus Status { get; set; }
    [Required]
    public DateTime CollectionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }

    public required UserProfile UserProfile { get; set; }
    public required Location Location { get; set; }
    public required Schedule Schedule { get; set; }
}

public enum BookingStatus
{
    Scheduled = 1,
    InTransit = 2,
    Collected = 3,
    Contaminated = 4,
    Cancelled = 5
}