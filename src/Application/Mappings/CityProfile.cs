using Application.DTOs.City;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, GetAllCityResponse>();
        CreateMap<City, GetByIdCityResponse>();

        CreateMap<CreateCityRequest, City>();
        CreateMap<UpdateCityRequest, City>();
    }
}
