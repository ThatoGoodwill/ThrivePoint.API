using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThrivePoint.Api.Data;
using ThrivePoint.Api.Models;

namespace ThrivePoint.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ThrivePointDbContext _db;
    public SettingsController(ThrivePointDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _db.CompanySettings.FindAsync(1) ?? new CompanySettings());

    [HttpPut]
    public async Task<IActionResult> Update(CompanySettings input)
    {
        var s = await _db.CompanySettings.FindAsync(1);
        if (s is null) { input.Id = 1; _db.CompanySettings.Add(input); s = input; }
        else
        {
            s.Company = input.Company; s.Email = input.Email;
            s.Phone = input.Phone; s.Currency = input.Currency;
            s.Notifications = input.Notifications; s.AutoAssign = input.AutoAssign;
        }
        await _db.SaveChangesAsync();
        return Ok(s);
    }
}