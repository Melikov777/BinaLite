namespace Application.DTOs.City;

public class GetAllCityResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; }
}
