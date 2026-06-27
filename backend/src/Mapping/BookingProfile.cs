using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<AddBookingRequest, Booking>();
        CreateMap<AddLocationRequest, Location>();
        CreateMap<AddRecyclingItemRequest, RecyclingItem>();
        CreateMap<AddScheduleRequest, Schedule>();
        CreateMap<AddUserProfileRequest, UserProfile>();

        CreateMap<UpdateBookingRequest, Booking>();
        CreateMap<UpdateLocationRequest, Location>();
        CreateMap<UpdateRecyclingItemRequest, RecyclingItem>();
        CreateMap<UpdateScheduleRequest, Schedule>();
        CreateMap<UpdateUserProfileRequest, UserProfile>();
    }
}