using Application.DTOs.City;

namespace Application.Abstracts.Services;

public interface ICityService
{
    Task<List<GetAllCityResponse>> GetAllCitiesAsync(CancellationToken ct = default);
    Task<GetByIdCityResponse> GetByIdCityAsync(int id, CancellationToken ct = default);
    Task CreateCityAsync(CreateCityRequest request, CancellationToken ct = default);
    void UpdateCity(UpdateCityRequest request);
    void DeleteCity(int id);
}
