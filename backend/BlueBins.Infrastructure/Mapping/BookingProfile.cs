using Application.Dtos;
using AutoMapper;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        var defaultMaterialQuantities = new Dictionary<MaterialTypes, int>
        {
            [MaterialTypes.tin] = 0,
            [MaterialTypes.aluminium] = 0,
            [MaterialTypes.glass] = 0,
        };
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
        .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.MaterialQuantities,
            opt => opt.MapFrom(src =>
             src.RecyclingItems == null ? defaultMaterialQuantities
            : src.RecyclingItems.ToDictionary(x => x.MaterialType, x => x.Quantity)));
    }
}