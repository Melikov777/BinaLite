using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.DTOs.City;
using AutoMapper;
using Domain.Entities;

namespace Persistence.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task CreateCityAsync(CreateCityRequest request, CancellationToken ct = default)
    {
        var city = _mapper.Map<City>(request);

        await _cityRepository.AddAsync(city, ct);
        await _cityRepository.SaveChangesAsync(ct);
    }

    public async Task<List<GetAllCityResponse>> GetAllCitiesAsync(CancellationToken ct = default)
    {
        var cities = await _cityRepository.GetAllAsync(ct);

        return _mapper.Map<List<GetAllCityResponse>>(cities);
    }

    public async Task<GetByIdCityResponse> GetByIdCityAsync(int id, CancellationToken ct = default)
    {
        var city = await _cityRepository.GetByIdAsync(id, ct);

        if (city == null)
            throw new Exception($"City with id {id} not found");

        return _mapper.Map<GetByIdCityResponse>(city);
    }

    public void UpdateCity(UpdateCityRequest request)
    {
        var city = _cityRepository.GetByIdAsync(request.Id).Result;

        if (city == null)
            throw new Exception($"City with id {request.Id} not found");

        _mapper.Map(request, city);

        _cityRepository.Update(city);
        _cityRepository.SaveChangesAsync().Wait();
    }

    public void DeleteCity(int id)
    {
        var city = _cityRepository.GetByIdAsync(id).Result;

        if (city == null)
            throw new Exception($"City with id {id} not found");

        _cityRepository.Delete(city);
        _cityRepository.SaveChangesAsync().Wait();
    }
}
