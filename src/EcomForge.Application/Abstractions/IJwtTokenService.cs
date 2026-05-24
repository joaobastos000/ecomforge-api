using EcomForge.Domain.Entities;

namespace EcomForge.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateAccessToken(Customer customer);
    string CreateRefreshToken();
    DateTime GetRefreshTokenExpirationUtc();
}
