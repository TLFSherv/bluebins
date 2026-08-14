public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;
    private readonly IBookingHelpers _helpers;
    public BookingService(IBookingRepository repository, IBookingHelpers helpers)
    {
        _repository = repository;
        _helpers = helpers;
    }

    public async Task<int?> AddBooking(string userId, BookingDTO booking)
    {
        List<AddRecyclingItemRequest>? recyclingItemRequests = null;
        if (booking.RecyclingItems != null)
        {
            recyclingItemRequests = new();
            foreach (var item in booking.RecyclingItems)
            {
                // use material type and quantity to calculate the weight and volume
                var (WeightKg, VolumeLiters) = _helpers.CalculateWeightAndVolume(item.MaterialType, item.ItemCount);
                recyclingItemRequests.Add(new AddRecyclingItemRequest
                {
                    MaterialType = item.MaterialType,
                    WeightKg = WeightKg,
                    VolumeLiters = VolumeLiters,
                    ContaminationPercent = 0
                });
            }
        }

        AddBookingRequest addBooking = new()
        {
            UserId = userId,
            Schedule = booking.Schedule,
            Status = booking.Status,
            CollectionDate = booking.CollectionDate,
            Location = booking.Location,
            RecyclingItems = recyclingItemRequests,
            DateCreated = DateTime.Now,
        };
        return await _repository.AddBooking(addBooking);
    }

    public async Task<BookingView?> GetBooking(string userId, int bookingId)
    {
        return await _repository.GetUserBooking(userId, bookingId);
    }

}
