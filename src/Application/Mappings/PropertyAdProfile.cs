using Application.DTOs.PropertyAd;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PropertyAdProfile : Profile
{
    public PropertyAdProfile()
    {
        CreateMap<PropertyMedia, PropertyMediaItemDto>();

        CreateMap<PropertyAd, GetAllPropertyAdResponse>()
            .ForMember(dest => dest.MainImageKey, opt => opt.MapFrom(src => 
                src.MediaItems.OrderBy(m => m.Order).Select(m => m.ObjectKey).FirstOrDefault()));

        CreateMap<PropertyAd, GetByIdPropertyAdResponse>()
            .ForMember(dest => dest.MediaItems, opt => opt.MapFrom(src => 
                src.MediaItems.OrderBy(m => m.Order)));

        CreateMap<CreatePropertyAdRequest, PropertyAd>();
        CreateMap<UpdatePropertyAdRequest, PropertyAd>();
    }
}
