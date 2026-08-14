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
    // Creates a new booking and returns the booking id
    public async Task<int?> AddBooking(AddBookingRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var locationId = request.Location?.LocationId;
            /* 
            conditions (location is not nullable): 
            - location id is null => create new location
            - location id not null => location already exists
            */
            if (locationId is null)
            {
                AddLocationRequest newLocation = new()
                {
                    MapsId = request.Location!.MapsId,
                    Address = request.Location.Address,
                    Postcode = request.Location.Postcode,
                    Latitude = request.Location.Latitude,
                    Longitude = request.Location.Longitude,
                    Details = request.Location.Details
                };
                locationId = await AddLocation(newLocation);
            }

            /* 
            conditions (schedule is nullable): 
            - schedule is not null and schedule id is null => create new schedule
            - schedule is null => there's no schedule
            - both schedule and schedule id not null => schedule already exists
            */
            var scheduleId = request?.Schedule?.ScheduleId;
            if (request?.Schedule is not null && scheduleId is null)
            {
                AddScheduleRequest addScheduleRequest = new() { StartDate = request.Schedule.StartDate, Frequency = request.Schedule.Frequency, IsActive = request.Schedule.IsActive };
                scheduleId = await AddSchedule(addScheduleRequest);
            }
            var userSchedule = await _context.Schedules.Where(x => x.Id == scheduleId).FirstOrDefaultAsync();

            Booking newBooking = new()
            {
                UserId = request.UserId,
                LocationId = locationId,
                ScheduleId = scheduleId,
                Status = request.Status,
                CollectionDate = request.CollectionDate,
                DateCreated = request.DateCreated,
                DateModified = request.DateModified,
            };
            _context.Add(newBooking);
            await _context.SaveChangesAsync();
            // add recycling items for booking
            if (request.RecyclingItems != null)
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
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex.Message);
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
        return await _context.Bookings
        .Where(b => b.UserId == userId && b.Id == bookingId)
        .Select(b =>
        new BookingView
        {
            Status = b.Status,
            CollectionDate = b.CollectionDate,
            DateCreated = b.DateCreated,
            DateModified = b.DateModified,
            Location = new LocationView
            {
                MapsId = b.Location!.MapsId,
                Address = b.Location.AddressLine1,
                Postcode = b.Location.Postcode,
                Latitude = b.Location.Latitude,
                Longitude = b.Location.Longitude,
                Details = b.Location.Details
            },
            Schedule = b.Schedule != null ? new ScheduleView
            {
                StartDate = b.Schedule.StartDate,
                Frequency = b.Schedule.Frequency,
                IsActive = b.Schedule.IsActive
            } : null,
            RecyclingItems = b.RecyclingItems != null ?
            b.RecyclingItems.Select(r => new RecyclingItemView
            {
                BookingId = r.BookingId,
                MaterialType = r.MaterialType,
                WeightKg = r.WeightKg,
                VolumeLiters = r.VolumeLiters,
                ContaminationPercent = r.ContaminationPercent
            }).ToList() : null
        }).FirstOrDefaultAsync();
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