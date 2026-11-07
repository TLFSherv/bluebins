using System.Collections;
using FluentAssertions.Extensions;

public class GetBookingByIdRepoTestData : IEnumerable<object?[]>
{
    public IEnumerator<object?[]> GetEnumerator()
    {
        BookingView expectedResult = new()
        {

            Status = BookingStatus.Scheduled,
            Schedule = new() { Date = new DateOnly(2026, 10, 2) },
            DateCreated = DateTime.Today.AsUtc(),
            Location = new()
            {
                MapsId = "test",
                Address = "test_address",
                Parish = "test_parish",
                Postcode = "test_postcode",
                Latitude = 0,
                Longitude = 0
            },
            Recycling = new()
            {
                BookingId = 1,
                NumberOfBags = 2,
                MaterialQuantities = new()
                {
                    [MaterialTypes.tin] = 5,
                    [MaterialTypes.aluminium] = 2,
                    [MaterialTypes.glass] = 7
                }
            }
        };
        yield return new object[] { 1, expectedResult }; // correct bookingId should return booking
        yield return new object?[] { 2, null }; // incorrect bookingId should return null
        yield return new object?[] { 3, null }; // incorrect bookingId should return null
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class CreateOrUpdateBookingTestData : IEnumerable<object?[]>
{
    public IEnumerator<object?[]> GetEnumerator()
    {
        LocationRequest location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 };
        RecyclingRequest quantity = new()
        {
            NumberOfBags = 2,
            MaterialQuantities = new()
            {
                {MaterialTypes.aluminium, 6},
                {MaterialTypes.glass, 3},
                {MaterialTypes.tin, 8},
            }
        };
        ScheduleRequest schedule1 = new() { Date = new DateOnly(2026, 10, 1), MakeDefault = true, Frequency = Frequency.Weekly };
        ScheduleRequest schedule2 = new() { Date = new DateOnly(2026, 10, 12), MakeDefault = false, Frequency = Frequency.Triweekly };
        BookingRequest request1 = new() { Location = location, Recycling = quantity, Schedule = schedule1 };
        BookingRequest request2 = new() { Location = location, Recycling = quantity, Schedule = schedule1 };
        BookingRequest request3 = new() { Location = location, Recycling = quantity, Schedule = schedule2 };
        yield return new object[] { true, request1 }; // request with no schedule
        yield return new object[] { true, request2 }; // request with existing schedule
        yield return new object[] { true, request3 }; // request with new schedule
        yield return new object[] { false, request1 }; // request with no schedule
        yield return new object[] { false, request2 }; // request with existing schedule
        yield return new object[] { false, request3 }; // request with new schedule
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class GetBookingRepoTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        BookingView view1 = new()
        {
            Status = BookingStatus.Scheduled,
            Location = new()
            {
                MapsId = "test",
                Address = "test_address",
                Parish = "test_parish",
                Postcode = "test_postcode",
                Latitude = 0,
                Longitude = 0
            },
            Schedule = new() { Date = new DateOnly(2026, 10, 2) },
            Recycling = new()
            {
                BookingId = 1,
                NumberOfBags = 2,
                MaterialQuantities = new()
                {
                    [MaterialTypes.tin] = 6,
                    [MaterialTypes.aluminium] = 4,
                    [MaterialTypes.glass] = 5
                }
            },
            DateCreated = DateTime.Today.AsUtc()
        };
        BookingView view2 = new()
        {
            Status = BookingStatus.Draft,
            Location = new()
            {
                MapsId = "test",
                Address = "test_address",
                Parish = "test_parish",
                Postcode = "test_postcode",
                Latitude = 0,
                Longitude = 0
            },
            Schedule = new() { Date = new DateOnly(2026, 10, 2) },
            Recycling = new()
        };
        BookingView view3 = new()
        {
            Status = BookingStatus.Draft,
            Location = new(),
            Schedule = new(),
            Recycling = new()
        };
        // User has an active booking
        yield return new object[] { "1", view1 };
        // User just has default data
        yield return new object[] { "2", view2 };
        // user has no booking and no default data
        yield return new object[] { "3", view3 };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

