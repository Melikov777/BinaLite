using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyAdController : ControllerBase
{
    private readonly IPropertyAdService _propertyAdService;

    public PropertyAdController(IPropertyAdService propertyAdService)
    {
        _propertyAdService = propertyAdService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _propertyAdService.GetAllPropertyAdsAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _propertyAdService.GetByIdPropertyAdAsync(id, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyAdRequest request, CancellationToken ct)
    {
        await _propertyAdService.CreatePropertyAdAsync(request, ct);
        return StatusCode(201);
    }

    [HttpPut]
    public IActionResult Update([FromBody] UpdatePropertyAdRequest request)
    {
        _propertyAdService.UpdatePropertyAd(request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _propertyAdService.DeletePropertyAd(id);
        return NoContent();
    }
}
