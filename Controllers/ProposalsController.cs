using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/proposals")]
[Authorize]
public class ProposalsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public ProposalsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Proposals
            .Include(p => p.Client).Include(p => p.Enquiry)
            .Include(p => p.Owner).Include(p => p.ServiceTags)
            .OrderByDescending(p => p.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Proposal p)
    {
        p.Id = 0;
        if (string.IsNullOrEmpty(p.Ref))
            p.Ref = $"P-{await _db.Proposals.CountAsync() + 2001}";
        p.CreatedAt = DateTime.UtcNow;
        _db.Proposals.Add(p);
        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Proposal input)
    {
        var p = await _db.Proposals.Include(x => x.ServiceTags).FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();

        p.ClientId = input.ClientId; p.EnquiryId = input.EnquiryId;
        p.Solution = input.Solution; p.Price = input.Price;
        p.Sent = input.Sent; p.Expiry = input.Expiry;
        p.Status = input.Status; p.OwnerId = input.OwnerId;
        p.Notes = input.Notes;

        p.ServiceTags.Clear();
        foreach (var tag in input.ServiceTags ?? new List<ProposalServiceTag>())
            p.ServiceTags.Add(new ProposalServiceTag { Name = tag.Name });

        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Proposals.FindAsync(id);
        if (p is null) return NotFound();
        _db.Proposals.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}