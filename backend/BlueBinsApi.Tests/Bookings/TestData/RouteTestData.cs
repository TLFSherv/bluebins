using System.Collections;
using System.Net;
using Application.Dtos;
public class CreateOrUpdateBookingRouteTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var materialQuantities = new Dictionary<MaterialTypes, int>()
        {
            {MaterialTypes.aluminium, 3} ,
            {MaterialTypes.glass, 2},
            {MaterialTypes.tin, 2},
        };
        BookingRequest request1 = new()
        {
            BookingDate = DateOnly.FromDateTime(DateTime.Today).AddDays(2),
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test", Latitude = -36.11m, Longitude = 21.44m },
            Schedule = new() { Frequency = Frequency.Weekly, DayOfWeek = DayOfWeek.Tuesday, MakeDefault = false },
            Recycling = new() { NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        BookingRequest request2 = new()
        {
            BookingDate = DateOnly.FromDateTime(DateTime.Today).AddDays(2),
            Schedule = new() { Frequency = Frequency.Biweekly, DayOfWeek = DayOfWeek.Thursday },
            Location = new() { MapsId = "test", Address = "a", Parish = "", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        yield return new object[] { request1, HttpStatusCode.Created, 1 };
        // Parish in Location is empty should fail validation
        yield return new object[] { request2, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class GetBookingRouteTestData : IEnumerable<object?[]>
{
    public IEnumerator<object?[]> GetEnumerator()
    {
        Dictionary<MaterialTypes, int> materialQuantities = new()
        {
            [MaterialTypes.tin] = 5,
            [MaterialTypes.aluminium] = 2,
            [MaterialTypes.glass] = 7
        };

        var booking = new BookingView()
        {
            Status = BookingStatus.Scheduled,
            BookingDate = new DateOnly(2026, 8, 20),
            Schedule = new() { Frequency = Frequency.Weekly, DayOfWeek = DayOfWeek.Wednesday },
            DateCreated = DateTime.Today,
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { BookingId = 1, NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        yield return new object[] { 1, booking };
        yield return new object?[] { 2, null }; // invalid booking id, should return bad request status code
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

