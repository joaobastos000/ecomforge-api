using EcomForge.Domain.Common;

namespace EcomForge.Domain.Entities;

public sealed class Customer : Entity
{
    private Customer() { }

    public Customer(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
    }

    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; private set; }

    public void SetRefreshToken(string token, DateTime expiresAtUtc)
    {
        RefreshToken = token;
        RefreshTokenExpiresAtUtc = expiresAtUtc;
    }
}
