using System.Collections;
using System.Net;
public class AddBookingRouteTestData : IEnumerable<object[]>
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
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test", Latitude = -36.11m, Longitude = 21.44m },
            Schedule = new() { Date = new DateOnly(2026, 11, 10), MakeDefault = false },
            Recycling = new() { NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        BookingRequest request2 = new()
        {
            Schedule = new() { Date = new DateOnly(2026, 11, 10) },
            Location = new() { MapsId = "test", Address = "test_address", Parish = "test_parish", Postcode = "test_postcode", Latitude = 0, Longitude = 0 },
            Recycling = new() { NumberOfBags = 2, MaterialQuantities = materialQuantities }
        };
        yield return new object[] { request1, HttpStatusCode.Created, 1 };
        // Latitude and Longitude in Location should fail validation
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
            Schedule = new() { Date = new DateOnly(2026, 8, 20) },
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

