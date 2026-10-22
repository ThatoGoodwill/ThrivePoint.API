using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Dtos;
using ThrivePoint.Api.Services;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    private readonly JwtTokenService _jwt;
    private readonly PermissionService _perms;

    public AuthController(ThrivePointDbContext db, JwtTokenService jwt, PermissionService perms)
    {
        _db = db; _jwt = jwt; _perms = perms;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            u.Email.ToLower() == request.Email.ToLower());

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var dto = new UserDto(user.Id, user.FullName, user.Email, user.Role, user.AvatarInitials);
        var perms = _perms.ForRole(user.Role);
        return Ok(new LoginResponse(_jwt.CreateToken(user), dto, perms));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<LoginResponse>> Me()
    {
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (idClaim is null) return Unauthorized();
        var user = await _db.Users.FindAsync(int.Parse(idClaim));
        if (user is null) return NotFound();

        var dto = new UserDto(user.Id, user.FullName, user.Email, user.Role, user.AvatarInitials);
        return Ok(new LoginResponse("", dto, _perms.ForRole(user.Role)));
    }
}