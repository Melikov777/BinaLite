using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PropertyAd;

public class GetByIdPropertyAdResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public double Price { get; set; }
    public bool IsExtract { get; set; }
    public bool IsMortgage { get; set; }
    public OfferType OfferType { get; set; }
    public RealEstateType RealEstateType { get; set; }
    public List<PropertyMediaItemDto> MediaItems { get; set; }
}