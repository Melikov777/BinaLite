using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Domain.Entities;

namespace Persistence.Services;

public class PropertyAdService : IPropertyAdService
{
    private readonly IPropertyAdRepository _propertyAdRepository;

    public PropertyAdService(IPropertyAdRepository propertyAdRepository)
    {
        _propertyAdRepository = propertyAdRepository;
    }

    public async Task CreatePropertyAdAsync(CreatePropertyAdRequest request, CancellationToken ct = default)
    {
        var propertyAd = new PropertyAd
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Location = request.Location,
            RoomCount = request.RoomCount,
            Area = request.Area,
            IsFurnished = request.IsFurnished,
            IsMortgage = request.IsMortgage,
            IsExtract = request.IsExtract,
            OfferType = request.OfferType,
            RealEstateType = request.RealEstateType
        };

        await _propertyAdRepository.AddAsync(propertyAd, ct);
        await _propertyAdRepository.SaveChangesAsync(ct);
    }

    public async Task<List<GetAllPropertyAdResponse>> GetAllPropertyAdsAsync(CancellationToken ct = default)
    {
        var propertyAds = await _propertyAdRepository.GetAllAsync(ct);

        return propertyAds.Select(x => new GetAllPropertyAdResponse
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description
        }).ToList();
    }

    public async Task<GetByIdPropertyAdResponse> GetByIdPropertyAdAsync(int id, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(id, ct);

        if (propertyAd == null)
            throw new Exception($"PropertyAd with id {id} not found");

        return new GetByIdPropertyAdResponse
        {
            Id = propertyAd.Id,
            Title = propertyAd.Title,
            Description = propertyAd.Description

        };
    }

    public void UpdatePropertyAd(UpdatePropertyAdRequest request)
    {
        var propertyAd = _propertyAdRepository.GetByIdAsync(request.Id).Result;

        if (propertyAd == null)
            throw new Exception($"PropertyAd with id {request.Id} not found");

        propertyAd.Title = request.Title;
        propertyAd.Description = request.Description;
        propertyAd.Price = request.Price;
        propertyAd.Location = request.Location;
        propertyAd.RoomCount = request.RoomCount;
        propertyAd.Area = request.Area;
        propertyAd.IsFurnished = request.IsFurnished;
        propertyAd.IsMortgage = request.IsMortgage;
        propertyAd.IsExtract = request.IsExtract;
        propertyAd.OfferType = request.OfferType;
        propertyAd.RealEstateType = request.RealEstateType;

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
