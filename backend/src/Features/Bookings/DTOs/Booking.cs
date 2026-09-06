public record BookingRequest : IRequest<string>
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public LocationRequest Location { get; set; }
    public ScheduleRequest Schedule { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public ICollection<RecyclingItemRequest>? RecyclingItems { get; set; }

}

public record BookingView
{
    public BookingStatus Status { get; set; }
    public DateTime CollectionDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public LocationView? Location { get; set; }
    public ScheduleView? Schedule { get; set; }
    public ICollection<RecyclingItemView>? RecyclingItems { get; set; }
};

