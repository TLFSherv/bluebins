public interface IDashboardRepository
{
    public Task<List<BookingDatesDto>> GetBookingDates(string userId);
}