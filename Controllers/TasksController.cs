using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public TasksController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.ProjectTasks
            .Include(t => t.Assignee)
            .Include(t => t.Project)
            .OrderByDescending(t => t.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ProjectTask t)
    {
        t.Id = 0;
        if (string.IsNullOrEmpty(t.Ref))
            t.Ref = $"T-{await _db.ProjectTasks.CountAsync() + 5001}";
        t.CreatedAt = DateTime.UtcNow;
        _db.ProjectTasks.Add(t);
        await _db.SaveChangesAsync();
        return Ok(t);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProjectTask input)
    {
        var t = await _db.ProjectTasks.FindAsync(id);
        if (t is null) return NotFound();

        t.Title = input.Title; t.ProjectId = input.ProjectId;
        t.AssigneeId = input.AssigneeId; t.Due = input.Due;
        t.Status = input.Status; t.Priority = input.Priority;
        t.Notes = input.Notes;

        await _db.SaveChangesAsync();
        return Ok(t);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.ProjectTasks.FindAsync(id);
        if (t is null) return NotFound();
        _db.ProjectTasks.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}