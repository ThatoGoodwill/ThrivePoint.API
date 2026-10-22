using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/docs")]
[Authorize]
public class DocsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public DocsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.ClientDocuments.OrderByDescending(d => d.Date).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ClientDocument d)
    {
        d.Id = 0;
        _db.ClientDocuments.Add(d);
        await _db.SaveChangesAsync();
        return Ok(d);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var d = await _db.ClientDocuments.FindAsync(id);
        if (d is null) return NotFound();
        _db.ClientDocuments.Remove(d);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}