using Application.DTOs.PropertyAd;

namespace Application.Abstracts.Services;

public interface IPropertyAdService
{
    Task<List<GetAllPropertyAdResponse>> GetAllPropertyAdsAsync(CancellationToken ct = default);
    Task<GetByIdPropertyAdResponse> GetByIdPropertyAdAsync(int id, CancellationToken ct = default);
    Task CreatePropertyAdAsync(CreatePropertyAdRequest request, string userId, CancellationToken ct = default);
    Task UpdatePropertyAdAsync(UpdatePropertyAdRequest request, string userId, CancellationToken ct = default);
    Task DeletePropertyAdAsync(int id, string userId, CancellationToken ct = default);
    Task<string> UploadMediaAsync(int propertyAdId, Microsoft.AspNetCore.Http.IFormFile file, CancellationToken ct = default);
    Task DeleteMediaAsync(int mediaId, CancellationToken ct = default);
    Task<List<Application.DTOs.PropertyMedia.PropertyMediaItemDto>> GetMediaForPropertyAdAsync(int propertyAdId, CancellationToken ct = default);
}
