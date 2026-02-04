namespace Domain.Entities;

public class District : BaseEntity<int>
{
    public string Name { get; set; }
    
    public int CityId { get; set; }
    
    public City City { get; set; }
}
