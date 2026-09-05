using System.ComponentModel.DataAnnotations;
using AutoMapper;

public record AddBookingRequest
{
    public string? UserId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public LocationDTO? Location { get; set; }
    public ScheduleDTO? Schedule { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public ICollection<AddRecyclingItemRequest>? RecyclingItems { get; set; }

}
public record UpdateBookingRequest : AddBookingRequest
{
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

