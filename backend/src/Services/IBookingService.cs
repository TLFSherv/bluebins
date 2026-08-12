public interface IBookingService
{
    public Task<int?> AddBooking(string userId, BookingDTO booking);
    public Task<BookingView?> GetBooking(string userId, int bookingId);
}