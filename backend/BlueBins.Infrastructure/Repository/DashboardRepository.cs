using Microsoft.EntityFrameworkCore;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<BookingDatesDto>> GetBookingDates(string userId)
    {
        return await _context.Bookings
        .Where(x => x.UserProfileId == userId)
        .Select(x => new BookingDatesDto
        {
            BookingId = x.Id,
            BookingDate = x.BookingDate
        }).ToListAsync();
    }
}