using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestAController : ControllerBase
{
    private readonly IPropertyAdService _propertyAdService;

    public TestAController(IPropertyAdService propertyAdService)
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
    public async Task<IActionResult> Create([FromBody] CreatePropertyAdRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.CreatePropertyAdAsync(request, userId!, ct);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePropertyAdRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.UpdatePropertyAdAsync(request, userId!, ct);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _propertyAdService.DeletePropertyAdAsync(id, userId!, ct);
        return Ok();
    }
}