using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public ServicesController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.ServiceOfferings.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ServiceOffering s)
    {
        s.Id = 0;
        _db.ServiceOfferings.Add(s);
        await _db.SaveChangesAsync();
        return Ok(s);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ServiceOffering input)
    {
        var s = await _db.ServiceOfferings.FindAsync(id);
        if (s is null) return NotFound();
        s.Name = input.Name; s.Type = input.Type;
        s.Price = input.Price; s.Cycle = input.Cycle;
        s.Description = input.Description;
        await _db.SaveChangesAsync();
        return Ok(s);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.ServiceOfferings.FindAsync(id);
        if (s is null) return NotFound();
        _db.ServiceOfferings.Remove(s);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}