using System.ComponentModel.DataAnnotations;

public record BookingDTO
{
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public required LocationDTO Location { get; set; }
    public required List<RecyclingItemDTO> RecyclingItems { get; set; }
    public ScheduleDTO? Schedule { get; set; }
}
public record AddBookingRequest
{
    public required string UserId { get; set; }
    public required BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public required LocationDTO Location { get; set; }
    public ScheduleDTO? Schedule { get; set; }
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