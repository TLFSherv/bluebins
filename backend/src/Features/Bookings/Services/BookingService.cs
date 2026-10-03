public class BookingService : IBookingService
{
    private readonly IHelperService _helper;
    private readonly IBookingRepository _repository;
    public BookingService(IHelperService helper, IBookingRepository repository)
    {
        _helper = helper;
        _repository = repository;

    }

    public async Task<int> AddUserBooking(CreateBookingRequest bookingRequest)
    {
        string userId = _helper.GetUserId();
        // Convert Create Booking request to Booking request
        List<RecyclingItemRequest> recyclingItemRequests = new();

        if (bookingRequest.Quantity.MaterialQuantities != null)
        {
            foreach (var (key, value) in bookingRequest.Quantity.MaterialQuantities)
            {
                recyclingItemRequests.Add(new RecyclingItemRequest(key, value));
            }
        }

        BookingRequest request = new()
        {
            Location = bookingRequest.Location,
            Schedule = bookingRequest.Schedule,
            Recycling = new()
            {
                NumberOfBags = bookingRequest.Quantity.NumberOfBags,
                RecyclingItems = recyclingItemRequests
            }
        };
        return await _repository.Add(userId, request);
    }

}