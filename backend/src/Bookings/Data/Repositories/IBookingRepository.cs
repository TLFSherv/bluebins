public interface IBookingRepository
{
    public Task<int?> AddLocation(AddLocationRequest request);
    public Task<int?> UpdateLocation(UpdateLocationRequest request);
    public Task<LocationView?> GetLocation(int locationId);

    public Task<int?> AddSchedule(AddScheduleRequest request);
    public Task<int?> UpdateSchedule(UpdateScheduleRequest request);
    public Task<ScheduleView?> GetSchedule(int scheduleId);

    public Task<int?> AddRecyclingItem(AddRecyclingItemRequest request);
    public Task<int?> UpdateRecyclingItem(UpdateRecyclingItemRequest request);
    public Task<RecyclingItemView?> GetRecyclingItem(int recyclingItemId);

    public Task<string?> AddUserProfile(AddUserProfileRequest request);
    public Task<string?> UpdateUserProfile(UpdateUserProfileRequest request);
    public Task<UserProfileView?> GetUserProfile(string userId);

    public Task<int?> AddBooking(AddBookingRequest request);
    public Task<int?> UpdateBooking(UpdateBookingRequest request);
    public Task<BookingView?> GetUserBooking(string userId, int bookingId);
}