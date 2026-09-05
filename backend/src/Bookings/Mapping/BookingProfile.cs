using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<BookingRequest, Booking>()
            // If Id > 0, assign ScheduleId. Otherwise, leave null/0 so EF relies on the navigation entity
            .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src =>
                (src.Schedule != null && src.Schedule.Id > 0) ? src.Schedule.Id : (int?)null))
            // If Id > 0, set Schedule to null (prevents tracker conflict).
            // If Id == 0, map the nested Schedule (EF Core will create the new record)     
            .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src =>
                (src.Schedule != null && src.Schedule.Id > 0) ? null : src.Schedule));
        CreateMap<LocationRequest, Location>();
        CreateMap<RecyclingItemRequest, RecyclingItem>();
        CreateMap<ScheduleRequest, Schedule>();
        CreateMap<UserProfileRequest, UserProfile>();

        CreateMap<Booking, BookingView>();
        CreateMap<Location, LocationView>();
        CreateMap<Schedule, ScheduleView>();
        CreateMap<RecyclingItem, RecyclingItemView>();
        CreateMap<UserProfile, UserProfileView>();
    }
}