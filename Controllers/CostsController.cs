using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/costs")]
[Authorize]
public class CostsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public CostsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.CostEntries.Include(c => c.Project)
            .OrderByDescending(c => c.Date).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CostEntry c)
    {
        c.Id = 0;
        _db.CostEntries.Add(c);
        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.CostEntries.FindAsync(id);
        if (c is null) return NotFound();
        _db.CostEntries.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}