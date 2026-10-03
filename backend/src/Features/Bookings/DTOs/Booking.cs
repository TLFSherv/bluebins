public record CreateBookingRequest : IRequest<int>
{
    public int Id { get; set; }
    public required LocationRequest Location { get; set; }
    public required ScheduleRequest Schedule { get; set; }
    public required QuantityRequest Quantity { get; set; }
}

public record BookingRequest : IRequest<int>
{
    public int Id { get; set; }
    public required LocationRequest Location { get; set; }
    public required ScheduleRequest Schedule { get; set; }
    public required RecyclingRequest Recycling { get; set; }

}

public record QuantityRequest
{
    public int NumberOfBags { get; set; }
    public Dictionary<MaterialTypes, int>? MaterialQuantities { get; set; }
}

public record UserBookingView
{
    public string UserId { get; set; }
    public Location? DefaultLocation { get; set; }
    public Schedule? DefaultSchedule { get; set; }
    public BookingView? ExistingBooking { get; set; }
}

public record BookingView
{
    public BookingStatus Status { get; set; }
    public required LocationView Location { get; set; }
    public required ScheduleView Schedule { get; set; }
    public required RecyclingView Recycling { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
};

