using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PinHistory, PinClean>()
            .ForMember(dest => dest.VendingBoxId, opt => opt.MapFrom(src => Enum.GetName(typeof(BoxLocation), src.VendingBoxId)));
    }
}