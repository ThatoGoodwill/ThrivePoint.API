using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Dtos;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/team")]
[Authorize]
public class TeamController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public TeamController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await _db.Users
            .Select(u => new UserDto(u.Id, u.FullName, u.Email, u.Role, u.AvatarInitials))
            .ToListAsync());
}