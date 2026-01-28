using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using AutoMapper;
using Domain.Entities;

namespace Persistence.Services;

public class PropertyAdService : IPropertyAdService
{
    private readonly IPropertyAdRepository _propertyAdRepository;
    private readonly IMapper _mapper;

    public PropertyAdService(IPropertyAdRepository propertyAdRepository, IMapper mapper)
    {
        _propertyAdRepository = propertyAdRepository;
        _mapper = mapper;
    }

    public async Task CreatePropertyAdAsync(CreatePropertyAdRequest request, CancellationToken ct = default)
    {
        var propertyAd = _mapper.Map<PropertyAd>(request);

        await _propertyAdRepository.AddAsync(propertyAd, ct);
        await _propertyAdRepository.SaveChangesAsync(ct);
    }

    public async Task<List<GetAllPropertyAdResponse>> GetAllPropertyAdsAsync(CancellationToken ct = default)
    {
        var propertyAds = await _propertyAdRepository.GetAllAsync(ct);

        return _mapper.Map<List<GetAllPropertyAdResponse>>(propertyAds);
    }

    public async Task<GetByIdPropertyAdResponse> GetByIdPropertyAdAsync(int id, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(id, ct);

        if (propertyAd == null)
            throw new Exception($"PropertyAd with id {id} not found");

        return _mapper.Map<GetByIdPropertyAdResponse>(propertyAd);
    }

    public void UpdatePropertyAd(UpdatePropertyAdRequest request)
    {
        var propertyAd = _propertyAdRepository.GetByIdAsync(request.Id).Result;

        if (propertyAd == null)
            throw new Exception($"PropertyAd with id {request.Id} not found");

        _mapper.Map(request, propertyAd);

        _propertyAdRepository.Update(propertyAd);
        _propertyAdRepository.SaveChangesAsync().Wait();
    }

    public void DeletePropertyAd(int id)
    {
        var propertyAd = _propertyAdRepository.GetByIdAsync(id).Result;

        if (propertyAd == null)
            throw new Exception($"PropertyAd with id {id} not found");

        _propertyAdRepository.Delete(propertyAd);
        _propertyAdRepository.SaveChangesAsync().Wait();
    }
}

