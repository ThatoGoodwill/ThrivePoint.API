using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/comms")]
[Authorize]
public class CommsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public CommsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Communications.Include(c => c.Owner)
            .OrderByDescending(c => c.Date).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Communication c)
    {
        c.Id = 0;
        _db.Communications.Add(c);
        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Communications.FindAsync(id);
        if (c is null) return NotFound();
        _db.Communications.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}