public class BookingService
{
    private readonly IBookingRepository _repository;
    public BookingService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<int?> AddBooking(string userId, BookingDTO booking)
    {
        List<AddRecyclingItemRequest>? recyclingItemRequests = new();
        foreach (var item in booking.RecyclingItems)
        {
            var (WeightKg, VolumeLiters) = CalculateItemProperties(item.MaterialType, item.ItemCount);
            recyclingItemRequests.Add(new AddRecyclingItemRequest
            {
                MaterialType = item.MaterialType,
                WeightKg = WeightKg,
                VolumeLiters = VolumeLiters,
                ContaminationPercent = 0
            });
        }

        AddBookingRequest bookingRequest = new()
        {
            UserId = userId,
            ScheduleId = booking.ScheduleId,
            Status = booking.Status,
            CollectionDate = booking.CollectionDate,
            Location = booking.Location,
            DateCreated = DateTime.Now,
            RecyclingItems = recyclingItemRequests
        };
        return await _repository.AddBooking(bookingRequest);
    }

    public async Task<BookingView?> GetBooking(string userId, int bookingId)
    {
        return await _repository.GetUserBooking(userId, bookingId);
    }

    public (decimal WeightKg, decimal VolumeLiters) CalculateItemProperties(MaterialTypes material, int itemCount)
    {
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        switch (material)
        {
            case MaterialTypes.tin:
                weightKg = materialWeights.tin * itemCount;
                break;
            case MaterialTypes.aluminum:
                weightKg = materialWeights.aluminum * itemCount;
                break;
            case MaterialTypes.glass:
                weightKg = materialWeights.glass * itemCount;
                break;
            case MaterialTypes.mixture:
                break;
        }
        return (weightKg, 0);
    }
}
