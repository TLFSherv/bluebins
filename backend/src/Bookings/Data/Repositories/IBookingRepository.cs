public interface IBookingRepository
{
    public Task<TId> AddEntity<TRequest, TEntity, TId>(TRequest requestDto) where TEntity : class, IEntity<TId>;
    public Task<int?> UpdateLocation(UpdateLocationRequest request);
    public Task<LocationView?> GetLocation(int locationId);

    public Task<int?> UpdateSchedule(UpdateScheduleRequest request);
    public Task<ScheduleView?> GetSchedule(int scheduleId);

    public Task<int?> UpdateRecyclingItem(UpdateRecyclingItemRequest request);
    public Task<RecyclingItemView?> GetRecyclingItem(int recyclingItemId);

    public Task<string?> UpdateUserProfile(UpdateUserProfileRequest request);
    public Task<UserProfileView?> GetUserProfile(string userId);

    public Task<int?> UpdateBooking(UpdateBookingRequest request);
    public Task<BookingView?> GetUserBooking(string userId, int bookingId);
}