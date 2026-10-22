using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/activity")]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public ActivityController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.ActivityLogs.OrderByDescending(a => a.Timestamp).Take(30).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Log(ActivityLog a)
    {
        a.Id = 0; a.Timestamp = DateTime.UtcNow;
        _db.ActivityLogs.Add(a);
        await _db.SaveChangesAsync();
        return Ok(a);
    }
}