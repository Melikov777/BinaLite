using Application.Abstracts.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyAdController : ControllerBase
{

    private readonly IRepository<PropertyAd, int> _repository;
    public PropertyAdController(IRepository<PropertyAd,int> repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var propertyAds = _repository.GetAll();
        return Ok(propertyAds);
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var propertyAd = _repository.GetById(id);
        if (propertyAd == null)
        {
            return NotFound();
        }
        return Ok(propertyAd);
    }
    [HttpPost]
    public IActionResult Create(PropertyAd propertyAd)
    {
        _repository.Add(propertyAd);
        return CreatedAtAction(nameof(GetById), new { id = propertyAd.Id }, propertyAd);
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, PropertyAd propertyAd)
    {
        if (id != propertyAd.Id)
        {
            return BadRequest();
        }
        _repository.Update(propertyAd);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
        {
        var propertyAd = _repository.GetById(id);
        if (propertyAd == null)
        {
            return NotFound();
        }
        _repository.Delete(propertyAd);
        return NoContent();
    }
    


}
