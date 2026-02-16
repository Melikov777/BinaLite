using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PropertyAdController : ControllerBase
{
    private readonly IPropertyAdService _propertyAdService;

    public PropertyAdController(IPropertyAdService propertyAdService)
    {
        _propertyAdService = propertyAdService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _propertyAdService.GetAllPropertyAdsAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _propertyAdService.GetByIdPropertyAdAsync(id, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePropertyAdRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.CreatePropertyAdAsync(request, userId!, ct);
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdatePropertyAdRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.UpdatePropertyAdAsync(request, userId!, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.DeletePropertyAdAsync(id, userId!, ct);
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
    [AllowAnonymous]
    public async Task<IActionResult> GetMedia(int id, CancellationToken ct)
    {
        var result = await _propertyAdService.GetMediaForPropertyAdAsync(id, ct);
        return Ok(result);
    }
}
