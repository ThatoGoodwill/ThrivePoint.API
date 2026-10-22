namespace ThrivePoint.Api.Services;

public class PermissionService
{
    private static readonly string[] AllModules =
    {
        "dashboard","enquiries","leads","discovery","proposals",
        "clients","projects","tasks","services",
        "finance","costs","comms","docs","reports",
        "team","settings"
    };

    private static readonly string[] TeamModules =
    {
        "dashboard","enquiries","leads","discovery","proposals",
        "clients","projects","tasks","services",
        "comms","docs","reports"
    };

    private static readonly Dictionary<string, string[]> Matrix = new()
    {
        ["founder"]   = AllModules,
        ["admin"]     = AllModules,
        ["marketing"] = TeamModules,
        ["developer"] = TeamModules,
        ["team"]      = TeamModules
    };

    public Dictionary<string, bool> ForRole(string role)
    {
        var key = (role ?? "team").ToLowerInvariant();
        var mods = Matrix.TryGetValue(key, out var m) ? m : TeamModules;
        return mods.ToDictionary(x => x, _ => true);
    }
}