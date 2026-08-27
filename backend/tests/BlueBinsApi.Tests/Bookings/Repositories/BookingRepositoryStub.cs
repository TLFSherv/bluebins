public class BookingRepositoryStub : IBookingRepository
{
    public async Task<int?> AddBooking(AddBookingRequest request)
    {
        return await Task.FromResult(1);
    }

    public async Task<int?> AddLocation(AddLocationRequest request)
    {
        return await Task.FromResult(1);
    }

    public async Task<int?> AddRecyclingItem(AddRecyclingItemRequest request)
    {
        return await Task.FromResult(1);
    }

    public async Task<int?> AddSchedule(AddScheduleRequest request)
    {
        return await Task.FromResult(1);
    }

    public async Task<string?> AddUserProfile(AddUserProfileRequest request)
    {
        return await Task.FromResult("1");
    }

    public async Task<LocationView?> GetLocation(int locationId)
    {
        throw new NotImplementedException();
    }

    public async Task<RecyclingItemView?> GetRecyclingItem(int recyclingItemId)
    {
        throw new NotImplementedException();
    }

    public async Task<ScheduleView?> GetSchedule(int scheduleId)
    {
        throw new NotImplementedException();
    }

    public async Task<BookingView?> GetUserBooking(string userId, int bookingId)
    {
        if (userId != "1" || bookingId != 1) return null;
        List<RecyclingItemView> recyclingItems = new()
        {
            new() {BookingId=1, MaterialType=MaterialTypes.aluminum, WeightKg=0.15m, VolumeLiters=0.3m, ContaminationPercent=0.1m},
            new() {BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.2m, VolumeLiters=0.1m, ContaminationPercent=0.3m},
            new() {BookingId=1, MaterialType=MaterialTypes.glass, WeightKg=0.1m, VolumeLiters=0.1m, ContaminationPercent=0.23m},
        };
        var booking = new BookingView()
        {
            Status = BookingStatus.Scheduled,
            CollectionDate = new DateTime(2026, 8, 20),
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", Address = "test_address", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            RecyclingItems = recyclingItems
        };

        return await Task.FromResult(booking);
    }

    public async Task<UserProfileView?> GetUserProfile(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> UpdateBooking(UpdateBookingRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> UpdateLocation(UpdateLocationRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> UpdateRecyclingItem(UpdateRecyclingItemRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> UpdateSchedule(UpdateScheduleRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> UpdateUserProfile(UpdateUserProfileRequest request)
    {
        throw new NotImplementedException();
    }
}