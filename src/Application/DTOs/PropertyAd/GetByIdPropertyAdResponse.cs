using Domain.Enums;

namespace Application.DTOs.PropertyAd;

public class GetByIdPropertyAdResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Location { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public bool IsFurnished { get; set; }
    public bool IsMortgage { get; set; }
    public bool IsExtract { get; set; }
    public OfferType OfferType { get; set; }
    public RealEstateType RealEstateType { get; set; }
    public List<PropertyMediaResponse> PropertyMedias { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PropertyMediaResponse
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string MediaType { get; set; }
}
