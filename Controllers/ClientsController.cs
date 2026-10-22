using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public ClientsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Clients.Include(c => c.ServiceTags).Include(c => c.Owner)
            .OrderByDescending(c => c.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Client c)
    {
        c.Id = 0;
        if (string.IsNullOrEmpty(c.Ref))
            c.Ref = $"C-{(await _db.Clients.CountAsync() + 1):D3}";
        c.CreatedAt = DateTime.UtcNow;
        _db.Clients.Add(c);
        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Client input)
    {
        var c = await _db.Clients.Include(x => x.ServiceTags).FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();

        c.Business = input.Business; c.Contact = input.Contact;
        c.Email = input.Email; c.Phone = input.Phone;
        c.Industry = input.Industry; c.Status = input.Status;
        c.ContractValue = input.ContractValue; c.PaidAmount = input.PaidAmount;
        c.OwnerId = input.OwnerId; c.Notes = input.Notes;

        c.ServiceTags.Clear();
        foreach (var tag in input.ServiceTags ?? new List<ClientServiceTag>())
            c.ServiceTags.Add(new ClientServiceTag { Name = tag.Name });

        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Clients.FindAsync(id);
        if (c is null) return NotFound();
        _db.Clients.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}