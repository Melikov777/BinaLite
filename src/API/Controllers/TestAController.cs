using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestAController : ControllerBase
{
    private readonly IPropertyAdService _propertyAdService;

    public TestAController(IPropertyAdService propertyAdService)
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
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePropertyAdRequest request, CancellationToken ct)
    {
        await _propertyAdService.UpdatePropertyAdAsync(request, ct);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _propertyAdService.DeletePropertyAdAsync(id, ct);
        return Ok();
    }
}
