using Application.Dtos;

public interface IBookingRepository
{
    public Task<BookingView?> GetBooking(string userId);
    public Task<BookingView?> GetBookingById(string userId, int bookingId);
    public Task<int> CreateOrUpdateBooking(string userId, BookingRequest bookingRequest);
}