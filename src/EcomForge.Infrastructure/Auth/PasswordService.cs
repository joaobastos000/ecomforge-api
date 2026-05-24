using EcomForge.Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace EcomForge.Infrastructure.Auth;

public sealed class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(new object(), password);
    }

    public bool Verify(string passwordHash, string password)
    {
        return _hasher.VerifyHashedPassword(new object(), passwordHash, password) != PasswordVerificationResult.Failed;
    }
}
