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
    public async Task<IActionResult> Create([FromForm] CreatePropertyAdRequest request, CancellationToken ct)
    {
        await _propertyAdService.CreatePropertyAdAsync(request, ct);
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdatePropertyAdRequest request, CancellationToken ct)
    {
        await _propertyAdService.UpdatePropertyAdAsync(request, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _propertyAdService.DeletePropertyAdAsync(id, ct);
        return NoContent();
    }
    [HttpPost("{id}/media")]
    public async Task<IActionResult> UploadMedia(int id, IFormFile file, CancellationToken ct)
    {
        var objectKey = await _propertyAdService.UploadMediaAsync(id, file, ct);
        return Ok(new { ObjectKey = objectKey });
    }

    [HttpDelete("media/{mediaId}")]
    public async Task<IActionResult> DeleteMedia(int mediaId, CancellationToken ct)
    {
        await _propertyAdService.DeleteMediaAsync(mediaId, ct);
        return NoContent();
    }

    [HttpGet("{id}/media")]
    public async Task<IActionResult> GetMedia(int id, CancellationToken ct)
    {
        var result = await _propertyAdService.GetMediaForPropertyAdAsync(id, ct);
        return Ok(result);
    }
}
