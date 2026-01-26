using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyAdController : ControllerBase
{
    private readonly IPropertyAdService _service;

    public PropertyAdController(IPropertyAdService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var propertyAds = _service.GetAll();
        return Ok(propertyAds);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var propertyAd = _service.GetById(id);
        if (propertyAd == null)
        {
            return NotFound();
        }
        return Ok(propertyAd);
    }

    [HttpPost]
    public IActionResult Create(CreatePropertyAdRequest request)
    {
        _service.Create(request);
        return StatusCode(201); // Created
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CreatePropertyAdRequest request)
    {
        _service.Update(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}
