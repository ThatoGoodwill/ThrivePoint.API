namespace ThrivePoint.Api.Dtos;

public record LoginRequest(string Email, string Password);

public record UserDto(int Id, string FullName, string Email, string Role, string? AvatarInitials);

public record LoginResponse(string Token, UserDto User, Dictionary<string, bool> Permissions);