using System.ComponentModel.DataAnnotations;

public record AddBookingRequest
{
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

}
public record UpdateBookingRequest : AddBookingRequest
{
    [Required]
    public int Id { get; set; }
}

public record BookingView : AddBookingRequest;