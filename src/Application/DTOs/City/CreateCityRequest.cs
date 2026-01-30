namespace Application.DTOs.City;

public class CreateCityRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; } = true;
}
