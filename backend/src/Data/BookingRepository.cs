using System.Transactions;
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
    public async Task<int?> AddBooking(AddBookingRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var locationId = request.Location?.LocationId;
            // if there's no location id create the location
            // if there is a location id then the user has selected their default location
            // and in that case it already exists in the database
            if (locationId is null)
            {
                Location newLocation = new()
                {
                    MapsId = request.Location.MapsId,
                    AddressLine1 = request.Location.Address,
                    Postcode = request.Location.Postcode,
                    Latitude = request.Location.Latitude,
                    Longitude = request.Location.Longitude,
                    Details = request.Location.Details
                };
                _context.Add(newLocation);
                await _context.SaveChangesAsync();
                locationId = newLocation.Id;
            }

            Booking newBooking = new()
            {
                UserId = request.UserId,
                LocationId = locationId,
                ScheduleId = request.ScheduleId,
                Status = request.Status,
                CollectionDate = request.CollectionDate,
                DateCreated = request.DateCreated,
                DateModified = request.DateModified
            };
            _context.Add(newBooking);
            await _context.SaveChangesAsync();

            if (request.RecyclingItems is not null)
            {
                List<RecyclingItem> newRecyclingItems = new();
                foreach (var item in request.RecyclingItems)
                {
                    newRecyclingItems.Add(new RecyclingItem()
                    {
                        BookingId = newBooking.Id,
                        MaterialType = item.MaterialType,
                        WeightKg = item.WeightKg,
                        VolumeLiters = item.VolumeLiters
                    });
                }
                _context.AddRange(newRecyclingItems);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return newBooking.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task<int?> AddLocation(AddLocationRequest request)
    {
        var location = _mapper.Map<AddLocationRequest, Location>(request);
        _context.Add(location);
        await _context.SaveChangesAsync();
        return location.Id;
    }

    public async Task<int?> AddRecyclingItem(AddRecyclingItemRequest request)
    {
        var recyclingItem = _mapper.Map<AddRecyclingItemRequest, RecyclingItem>(request);
        _context.Add(recyclingItem);
        await _context.SaveChangesAsync();
        return recyclingItem.Id;
    }

    public async Task<int?> AddSchedule(AddScheduleRequest request)
    {
        var schedule = _mapper.Map<AddScheduleRequest, Schedule>(request);
        _context.Add(schedule);
        await _context.SaveChangesAsync();
        return schedule.Id;
    }

    public async Task<string?> AddUserProfile(AddUserProfileRequest request)
    {
        var userProfile = _mapper.Map<AddUserProfileRequest, UserProfile>(request);
        _context.Add(userProfile);
        await _context.SaveChangesAsync();
        return userProfile.Id;
    }

    public async Task<BookingView?> GetUserBooking(string userId, int bookingId)
    {
        // utilise navigation properties to access data from tables with fk rather than joins
        var result = await _context.Bookings
        .Where(b => b.UserId == userId && b.Id == bookingId)
        .Select(b =>
        new BookingView
        {
            Status = b.Status,
            CollectionDate = b.CollectionDate,
            DateCreated = b.DateCreated,
            DateModified = b.DateModified,
            Location = b.Location != null ? new LocationView
            {
                MapsId = b.Location.MapsId,
                Address = b.Location.AddressLine1,
                Latitude = b.Location.Latitude,
                Longitude = b.Location.Longitude,
                Details = b.Location.Details
            } : null,
            Schedule = b.Schedule != null ? new ScheduleView
            {
                StartDate = b.Schedule.StartDate,
                Frequency = b.Schedule.Frequency,
                IsActive = b.Schedule.IsActive
            } : null
        }).SingleOrDefaultAsync();

        if (result != null)
        {
            var recyclingItems = await _context.RecyclingItems
            .Where(r => r.BookingId == bookingId)
            .Select(r => new RecyclingItemView
            {
                BookingId = r.BookingId,
                MaterialType = r.MaterialType,
                WeightKg = r.WeightKg,
                VolumeLiters = r.VolumeLiters,
                ContaminationPercent = r.ContaminationPercent
            }).ToListAsync();
            result.RecyclingItems = recyclingItems;
        }

        return result;
    }

    public async Task<LocationView?> GetLocation(int locationId)
    {
        return await _context.Locations
        .Where(x => x.Id == locationId)
        .Select(x => new LocationView
        {
            MapsId = x.MapsId,
            Address = x.AddressLine1,
            Postcode = x.Postcode,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            Details = x.Details
        }).SingleOrDefaultAsync();
    }

    public async Task<RecyclingItemView?> GetRecyclingItem(int recyclingItemId)
    {
        return await _context.RecyclingItems
        .Where(x => x.Id == recyclingItemId)
        .Select(x => new RecyclingItemView
        {
            MaterialType = x.MaterialType,
            WeightKg = x.WeightKg,
            VolumeLiters = x.VolumeLiters,
            ContaminationPercent = x.ContaminationPercent
        }).SingleOrDefaultAsync();
    }

    public async Task<ScheduleView?> GetSchedule(int scheduleId)
    {
        return await _context.Schedules
        .Where(x => x.Id == scheduleId)
        .Select(x => new ScheduleView
        {
            StartDate = x.StartDate,
            Frequency = x.Frequency,
            IsActive = x.IsActive
        }).SingleOrDefaultAsync();
    }

    public async Task<UserProfileView?> GetUserProfile(string userId)
    {
        return await _context.UserProfiles
        .Where(x => x.Id == userId)
        .Select(x => new UserProfileView
        {
            DefaultLocationId = x.DefaultLocationId,
            DefaultScheduleId = x.DefaultScheduleId,
            IsDeleted = x.IsDeleted
        }).SingleOrDefaultAsync();
    }

    public async Task<int?> UpdateBooking(UpdateBookingRequest request)
    {
        var booking = await _context.Bookings.FindAsync(request.Id);

        if (booking is null)
        {
            throw new Exception("Unable to find the booking");
        }

        _mapper.Map<UpdateBookingRequest, Booking>(request);

        await _context.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<int?> UpdateLocation(UpdateLocationRequest request)
    {
        var location = await _context.Locations.FindAsync(request.LocationId);

        if (location is null)
        {
            throw new Exception("Unable to find location");
        }

        _mapper.Map<UpdateLocationRequest, Location>(request);

        await _context.SaveChangesAsync();
        return location.Id;
    }

    public async Task<int?> UpdateRecyclingItem(UpdateRecyclingItemRequest request)
    {
        var recyclingItem = await _context.RecyclingItems.FindAsync(request.Id);

        if (recyclingItem is null)
        {
            throw new Exception("Unable to find recycling item");
        }

        _mapper.Map<UpdateRecyclingItemRequest, RecyclingItem>(request);
        await _context.SaveChangesAsync();
        return recyclingItem.Id;
    }

    public async Task<int?> UpdateSchedule(UpdateScheduleRequest request)
    {
        var schedule = await _context.Schedules.FindAsync(request.ScheduleId);

        if (schedule is null)
        {
            throw new Exception("Unable to find schedule");
        }

        _mapper.Map<UpdateScheduleRequest, Schedule>(request);
        await _context.SaveChangesAsync();
        return schedule.Id;
    }

    public async Task<string?> UpdateUserProfile(UpdateUserProfileRequest request)
    {
        var userProfile = await _context.UserProfiles.FindAsync(request.Id);

        if (userProfile is null)
        {
            throw new Exception("Unable to find user profile");
        }

        _mapper.Map<UpdateUserProfileRequest, UserProfile>(request);
        await _context.SaveChangesAsync();
        return userProfile.Id;
    }
}