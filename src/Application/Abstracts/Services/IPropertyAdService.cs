using Application.DTOs.PropertyAd;
using Domain.Enums;

namespace Application.Abstracts.Services;

public interface IPropertyAdService
{
    Task<List<GetAllPropertyAdResponse>> GetAllPropertyAdsAsync(CancellationToken ct=default);
    Task<GetByIdPropertyAdResponse> GetByIdPropertyAdAsync(int id, CancellationToken ct=default);
    Task CreatePropertyAdAsync(CreatePropertyAdRequest request, CancellationToken ct=default);
    void UpdatePropertyAd(UpdatePropertyAdRequest request);
    void DeletePropertyAd(int id);
}
