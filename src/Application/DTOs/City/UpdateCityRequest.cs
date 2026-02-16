namespace Application.DTOs.City;

public class UpdateCityRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; }
}