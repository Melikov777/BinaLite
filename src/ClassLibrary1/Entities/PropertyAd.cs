using Domain.Enums;

namespace Domain.Entities;

public class PropertyAd: BaseEntity<int>
{
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
    public ICollection<PropertyMedia> PropertyMedias { get; set; }
}
