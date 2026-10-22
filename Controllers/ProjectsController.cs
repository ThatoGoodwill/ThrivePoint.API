using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public ProjectsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Projects
            .Include(p => p.Client)
            .Include(p => p.Team).ThenInclude(t => t.User)
            .OrderByDescending(p => p.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Project p)
    {
        p.Id = 0;
        if (string.IsNullOrEmpty(p.Ref))
            p.Ref = $"PRJ-{await _db.Projects.CountAsync() + 301}";
        p.CreatedAt = DateTime.UtcNow;
        _db.Projects.Add(p);
        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Project input)
    {
        var p = await _db.Projects.Include(x => x.Team).FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();

        p.Name = input.Name; p.ClientId = input.ClientId;
        p.Start = input.Start; p.Deadline = input.Deadline;
        p.Status = input.Status; p.Progress = input.Progress;
        p.Value = input.Value; p.Notes = input.Notes;

        p.Team.Clear();
        foreach (var m in input.Team ?? new List<ProjectTeamMember>())
            p.Team.Add(new ProjectTeamMember { UserId = m.UserId });

        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Projects.FindAsync(id);
        if (p is null) return NotFound();
        _db.Projects.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}