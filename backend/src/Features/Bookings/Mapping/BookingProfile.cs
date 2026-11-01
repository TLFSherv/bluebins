using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        // Used in booking repository
        CreateMap<BookingRequest, Booking>();
        CreateMap<LocationRequest, Location>();
        CreateMap<ScheduleRequest, Schedule>();
        CreateMap<RecyclingRequest, Recycling>()
        .ForMember(dest => dest.RecyclingItems,
        opt => opt.MapFrom(src => src.GetRecyclingItems()));

        CreateMap<Booking, BookingView>();
        CreateMap<Location, LocationView>();
        CreateMap<Schedule, ScheduleView>();
        CreateMap<Recycling, RecyclingView>()
        .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id));
        CreateMap<RecyclingItem, RecyclingItemView>();
    }
}