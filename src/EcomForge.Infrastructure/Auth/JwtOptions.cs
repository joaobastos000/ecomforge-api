namespace EcomForge.Infrastructure.Auth;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = "EcomForge";
    public string Audience { get; init; } = "EcomForge.Client";
    public string Secret { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 30;
    public int RefreshTokenDays { get; init; } = 7;
}
