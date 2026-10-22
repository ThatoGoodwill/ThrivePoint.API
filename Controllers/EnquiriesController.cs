using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/enquiries")]
[Authorize]
public class EnquiriesController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public EnquiriesController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Enquiries.Include(e => e.Owner)
            .OrderByDescending(e => e.CreatedAt).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var e = await _db.Enquiries.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Enquiry e)
    {
        e.Id = 0;
        if (string.IsNullOrEmpty(e.Ref))
            e.Ref = $"E-{(await _db.Enquiries.CountAsync() + 1):D3}";
        e.CreatedAt = e.UpdatedAt = DateTime.UtcNow;
        _db.Enquiries.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Enquiry input)
    {
        var e = await _db.Enquiries.FindAsync(id);
        if (e is null) return NotFound();

        e.Business = input.Business; e.Contact = input.Contact;
        e.Email = input.Email; e.Phone = input.Phone;
        e.Industry = input.Industry; e.Location = input.Location;
        e.Situation = input.Situation; e.Challenge = input.Challenge;
        e.Goals = input.Goals; e.Tried = input.Tried; e.Support = input.Support;
        e.Status = input.Status; e.Priority = input.Priority;
        e.Source = input.Source; e.Timeframe = input.Timeframe;
        e.Value = input.Value; e.OwnerId = input.OwnerId;
        e.FollowUpDate = input.FollowUpDate;
        e.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Enquiries.FindAsync(id);
        if (e is null) return NotFound();
        _db.Enquiries.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}