public interface IBookingRepository
{
    public Task<BookingView?> GetBooking(string userId);
    public Task<BookingView?> GetBookingById(string userId, int bookingId);
    public Task<int> AddBooking(string userId, BookingRequest bookingRequest);
    public Task<int> UpdateBooking(string userId, BookingRequest bookingRequest);
}