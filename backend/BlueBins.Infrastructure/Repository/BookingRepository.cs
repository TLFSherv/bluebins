using Application.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public BookingRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<BookingView?> GetBooking(string userId)
    {
        // Two scenarios: 
        // 1. User has an active booking and in that case just return it
        // 2. User doesn't have an active booking so return new booking with default data

        // 1. Return users active booking
        Booking? booking = await _context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.UserProfileId == userId &&
        (x.Status == BookingStatus.Scheduled ||
        x.Status == BookingStatus.Draft));

        if (booking != null)
        {
            // If a draft bookings schedule date has passed, set the date to tomorrow and reset frequency to once
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            if (booking.Schedule.Date <= today)
            {
                booking.Schedule.Date = today.AddDays(1);
                booking.Schedule.Frequency = Frequency.Once;
            }
            return _mapper.Map<BookingView>(booking);
        }

        // 2. Return new booking with users default data
        UserProfile userProfile = await _context.UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .AsNoTracking()
        .SingleAsync(x => x.Id == userId);

        return new()
        {
            Location = (userProfile.DefaultLocation != null) ?
                _mapper.Map<LocationView>(userProfile.DefaultLocation) : new(),
            Schedule = (userProfile.DefaultSchedule != null) ?
                _mapper.Map<ScheduleView>(userProfile.DefaultSchedule) : new(),
            Recycling = new(),
            Status = BookingStatus.Draft
        };
    }

    public async Task<BookingView?> GetBookingById(string userId, int bookingId)
    {
        var booking = await _context.Bookings
        .Include(x => x.Location)
        .Include(x => x.Schedule)
        .Include(x => x.Recycling)
        .ThenInclude(x => x.RecyclingItems)
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.UserProfileId == userId && x.Id == bookingId);

        return (booking != null) ?
        _mapper.Map<BookingView>(booking) : null;
    }
    public async Task<int> CreateOrUpdateBooking(string userId, BookingRequest bookingRequest)
    {

        // Check for current active booking
        Booking? currBooking = null;
        int bookingId = await _context.Bookings
        .Where(x => x.UserProfileId == userId &&
        (x.Status == BookingStatus.Scheduled ||
        x.Status == BookingStatus.Draft))
        .Select(x => x.Id).SingleOrDefaultAsync();

        // Map the request
        var newBooking = _mapper.Map<Booking>(bookingRequest);
        newBooking.UserProfileId = userId;

        if (bookingId != 0)
        {
            // Get existing booking and its relationships
            currBooking = await _context.Bookings
            .Include(x => x.Location)
            .Include(x => x.Schedule)
            .Include(x => x.Recycling)
            .ThenInclude(x => x.RecyclingItems)
            .SingleAsync(x => x.Id == bookingId);
            // Replace recycling data
            currBooking.Recycling = newBooking.Recycling;
        }

        var booking = currBooking ?? newBooking;

        // Check if the location and schedule already exist
        var foundLocation = await _context.Locations
        .FirstOrDefaultAsync(x => x.Address == newBooking.Location.Address);

        var foundSchedule = await _context.Schedules
        .FirstOrDefaultAsync(x => x.Date == newBooking.Schedule.Date
        && x.Frequency == newBooking.Schedule.Frequency);

        if (newBooking.Schedule.Frequency == Frequency.Monthly)
        {
            // compare the date
        }
        else
        {
            // compare the day of the week and frequency
        }

        booking.ChangeLocationAndSchedule(
            foundLocation ?? newBooking.Location,
            foundSchedule ?? newBooking.Schedule
        );
        // If we're adding a new booking add the booking to change tracker
        if (currBooking == null)
        {
            _context.Add(booking);
        }

        // Change users default preferences
        var user = await _context
        .UserProfiles
        .Include(x => x.DefaultLocation)
        .Include(x => x.DefaultSchedule)
        .SingleAsync(x => x.Id == userId);

        if (bookingRequest.Schedule.MakeDefault)
        {
            user.DefaultSchedule = booking.Schedule;
        }
        if (bookingRequest.Location.MakeDefault)
        {
            user.DefaultLocation = booking.Location;
        }
        await _context.SaveChangesAsync();
        return booking.Id;
    }
}