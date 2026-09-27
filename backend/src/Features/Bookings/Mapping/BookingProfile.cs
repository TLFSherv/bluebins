using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<LocationRequest, Location>();
        CreateMap<RecyclingRequest, Recycling>()
        .ForMember(dest => dest.RecyclingItems, opt => opt.MapFrom(src => src.GetRecyclingItems()));
        CreateMap<ScheduleRequest, Schedule>();
        CreateMap<UserProfileRequest, UserProfile>();

        CreateMap<Booking, BookingView>();
        CreateMap<Location, LocationView>();
        CreateMap<Schedule, ScheduleView>();
        CreateMap<Recycling, RecyclingView>()
        .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id));
        CreateMap<UserProfile, UserProfileView>();
        CreateMap<RecyclingItem, RecyclingItemView>();
    }
}