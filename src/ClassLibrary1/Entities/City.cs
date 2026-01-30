namespace Domain.Entities;

public class City : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } 
    public string Country { get; set; } 
    public bool IsActive { get; set; } = true;

}
