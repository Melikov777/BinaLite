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

    public List<GetAllPropertyAdResponse> GetAll()
    {
        var propertyAds = _propertyAdRepository.GetAllWithMedia();
        return propertyAds.Select(p => new GetAllPropertyAdResponse
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            Location = p.Location,
            RoomCount = p.RoomCount,
            Area = p.Area,
            IsFurnished = p.IsFurnished,
            IsMortgage = p.IsMortgage,
            IsExtract = p.IsExtract,
            OfferType = p.OfferType,
            RealEstateType = p.RealEstateType,
            CreatedAt = p.CreatedDate
        }).ToList();
    }

    public GetByIdPropertyAdResponse GetById(int id)
    {
        var propertyAd = _propertyAdRepository.GetByIdWithMedia(id);
        if (propertyAd == null) return null;

        return new GetByIdPropertyAdResponse
        {
            Id = propertyAd.Id,
            Title = propertyAd.Title,
            Description = propertyAd.Description,
            Price = propertyAd.Price,
            Location = propertyAd.Location,
            RoomCount = propertyAd.RoomCount,
            Area = propertyAd.Area,
            IsFurnished = propertyAd.IsFurnished,
            IsMortgage = propertyAd.IsMortgage,
            IsExtract = propertyAd.IsExtract,
            OfferType = propertyAd.OfferType,
            RealEstateType = propertyAd.RealEstateType,
            PropertyMedias = propertyAd.PropertyMedias?.Select(m => new PropertyMediaResponse
            {
                Id = m.Id,
                Url = m.MediaUrl,
                MediaType = m.MediaType
            }).ToList(),
            CreatedAt = propertyAd.CreatedDate,
            UpdatedAt = propertyAd.ModifiedDate
        };
    }

    public void Create(CreatePropertyAdRequest request)
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
            RealEstateType = request.RealEstateType,
            CreatedDate = DateTime.UtcNow
        };

        _propertyAdRepository.Add(propertyAd);
        _propertyAdRepository.SaveChanges();
    }

    public void Update(int id, CreatePropertyAdRequest request)
    {
        var propertyAd = _propertyAdRepository.GetById(id);
        if (propertyAd == null) return;

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
        propertyAd.ModifiedDate = DateTime.UtcNow;

        _propertyAdRepository.Update(propertyAd);
        _propertyAdRepository.SaveChanges();
    }

    public void Delete(int id)
    {
        var propertyAd = _propertyAdRepository.GetById(id);
        if (propertyAd == null) return;

        _propertyAdRepository.Delete(propertyAd);
        _propertyAdRepository.SaveChanges();
    }
}

