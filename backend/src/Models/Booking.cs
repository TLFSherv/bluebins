using System.ComponentModel.DataAnnotations;

public class Booking
{
    [Required]
    public int? BookingId { get; set; }
    [Required]
    public int? UserId { get; set; }
    [Required]
    public int? LocationId { get; set; }
    [Required]
    public int? ScheduleId { get; set; }
    [Required]
    public BookingStatus Status { get; set; }
    [Required]
    public DateTime CollectionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}

public enum BookingStatus
{
    Scheduled = 1,
    InTransit = 2,
    Collected = 3,
    Contaminated = 4,
    Cancelled = 5
}