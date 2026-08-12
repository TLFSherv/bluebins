using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class BookingRepositoryTests
{
    [Fact]
    public async Task GetBookingSuccessfully()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlite(connection)
        .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureCreated();

            Schedule schedule = new Schedule
            {
                Id = 1,
                StartDate = new DateOnly(2026, 6, 28),
                Frequency = FrequencyTypes.Weekly,
                IsActive = true
            };

            Location location = new Location
            {
                Id = 1,
                AddressLine1 = "TestAddress",
                Postcode = "Test postcode",
                Latitude = 37.111M,
                Longitude = -16.111M,
                Details = "This is a test"
            };

            context.UserProfiles.Add(
                new UserProfile
                {
                    Id = "test",
                    IsDeleted = false
                }
            );

            context.Bookings.Add(
                new Booking
                {
                    Id = 1,
                    UserId = "test",
                    LocationId = 1,
                    ScheduleId = 1,
                    Schedule = schedule,
                    Location = location,
                    Status = BookingStatus.Scheduled,
                    CollectionDate = new DateTime(2026, 6, 29),
                    DateCreated = new DateTime(2026, 6, 28),
                }
            );

            context.RecyclingItems.AddRange(
                new RecyclingItem { Id = 1, BookingId = 1, MaterialType = MaterialTypes.tin, WeightKg = 2.3M, VolumeLiters = 0.23M, ContaminationPercent = 0.2M },
                new RecyclingItem { Id = 2, BookingId = 1, MaterialType = MaterialTypes.glass, WeightKg = 2.3M, VolumeLiters = 0.23M, ContaminationPercent = 0.2M },
                new RecyclingItem { Id = 3, BookingId = 1, MaterialType = MaterialTypes.aluminum, WeightKg = 2.3M, VolumeLiters = 0.23M, ContaminationPercent = 0.2M }
            );
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var bookingProfile = new BookingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(bookingProfile), NullLoggerFactory.Instance);
            var mapper = new Mapper(configuration);
            var repository = new BookingRepository(context, mapper);

            var booking = await repository.GetUserBooking("test", 1);

            Assert.NotNull(booking);
            Assert.IsType<BookingView>(booking);
            Assert.Equal("TestAddress", booking?.Location?.Address);
            Assert.Equal(3, booking?.RecyclingItems?.Count);
        }
        connection.Close();
    }

    [Fact]
    public async Task AddBookingSuccessfully()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlite(connection)
        .LogTo(message => Console.WriteLine(message), Microsoft.Extensions.Logging.LogLevel.Error)
        .EnableSensitiveDataLogging()
        .Options;
        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureCreated();
            context.UserProfiles.Add(
              new UserProfile
              {
                  Id = "testuser",
                  IsDeleted = false
              }
          );
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            AddBookingRequest request = new()
            {
                UserId = "testuser",
                Status = BookingStatus.Scheduled,
                CollectionDate = new DateTime(2026, 7, 2),
                Location = new() { LocationId = null, Address = "testaddress", Postcode = "testpostcode", Latitude = 0M, Longitude = 0M },
                DateCreated = DateTime.Now,
                RecyclingItems = new List<AddRecyclingItemRequest>()
                {
                    new() {MaterialType = MaterialTypes.mixture, WeightKg=0,VolumeLiters=0,ContaminationPercent=0}
                }
            };

            var bookingProfile = new BookingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(bookingProfile), NullLoggerFactory.Instance);
            var mapper = new Mapper(configuration);
            var repository = new BookingRepository(context, mapper);

            var result = await repository.AddBooking(request);

            Assert.NotNull(result);
            Assert.IsType<int>(result);
        }
        connection.Close();
    }
}