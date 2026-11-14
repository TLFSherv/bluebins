namespace Application.Dtos;

public record BookingRequest
{
    public int Id { get; set; }
    public required DateOnly BookingDate { get; set; }
    public required LocationRequest Location { get; set; }
    public ScheduleRequest? Schedule { get; set; }
    public required RecyclingRequest Recycling { get; set; }

}

public record BookingView
{
    public DateOnly BookingDate { get; set; }
    public BookingStatus Status { get; set; }
    public required LocationView Location { get; set; }
    public ScheduleView? Schedule { get; set; }
    public required RecyclingView Recycling { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
};

