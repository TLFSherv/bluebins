using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    [Required]
    public int Id { get; set; }
    public string? UserId { get; set; }
    [Required]
    public int? LocationId { get; set; }
    public int? ScheduleId { get; set; }
    [Required]
    public BookingStatus Status { get; set; }
    [Required]
    public DateTime CollectionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }

    public UserProfile? UserProfile { get; set; }
    public Location? Location { get; set; }
    public Schedule? Schedule { get; set; }
    public ICollection<RecyclingItem>? RecyclingItems { get; set; }
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