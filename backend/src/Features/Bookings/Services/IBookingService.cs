public interface IBookingService
{
    public Task<int> AddUserBooking(CreateBookingRequest bookingRequest);
}