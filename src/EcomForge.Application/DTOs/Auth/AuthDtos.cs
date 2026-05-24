namespace EcomForge.Application.DTOs.Auth;

public sealed record RegisterRequest(string Name, string Email, string Password);
public sealed record LoginRequest(string Email, string Password);
public sealed record RefreshTokenRequest(Guid CustomerId, string RefreshToken);
public sealed record AuthResponse(Guid CustomerId, string Name, string Email, string AccessToken, string RefreshToken);
