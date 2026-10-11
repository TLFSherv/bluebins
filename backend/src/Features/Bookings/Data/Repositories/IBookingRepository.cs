public interface IBookingRepository : IRepository
{
    public Task<BookingView?> GetUserBooking(string userId);
    public Task<int> Add(string userId, BookingRequest bookingRequest);
    public Task<int> Update(BookingRequest bookingRequest);
}