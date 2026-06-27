using System.ComponentModel.DataAnnotations;

public record BookingDTO
{
    public int? ScheduleId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public LocationDTO Location { get; set; }
    public List<RecyclingItemDTO> RecyclingItems { get; set; }
}
public record AddBookingRequest
{
    public string? UserId { get; set; }
    public int? ScheduleId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public LocationDTO Location { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public List<AddRecyclingItemRequest>? RecyclingItems { get; set; }

}
public record UpdateBookingRequest : AddBookingRequest
{
    [Required]
    public string? Id { get; set; }
}

public record BookingView
{
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public LocationView? Location { get; set; }
    public ScheduleView? Schedule { get; set; }
    public List<RecyclingItemView>? RecyclingItems { get; set; }
};