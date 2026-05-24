using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Auth;
using EcomForge.Common.Results;
using EcomForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomForge.Application.Services;

public sealed class AuthService(IAppDbContext dbContext, IJwtTokenService jwtTokenService, IPasswordService passwordService)
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.ToLowerInvariant();
        var exists = await dbContext.Customers.AnyAsync(x => x.Email == email, cancellationToken);
        if (exists)
        {
            return Result<AuthResponse>.Failure(new Error("Auth.EmailInUse", "Email already registered."));
        }

        var customer = new Customer(request.Name, email, passwordService.Hash(request.Password));

        SetRefreshToken(customer);
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(CreateResponse(customer));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.ToLowerInvariant();
        var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (customer is null)
        {
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidCredentials", "Invalid credentials."));
        }

        if (!passwordService.Verify(customer.PasswordHash, request.Password))
        {
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidCredentials", "Invalid credentials."));
        }

        SetRefreshToken(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(CreateResponse(customer));
    }

    public async Task<Result<AuthResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer is null ||
            customer.RefreshToken != request.RefreshToken ||
            customer.RefreshTokenExpiresAtUtc <= DateTime.UtcNow)
        {
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidRefreshToken", "Invalid refresh token."));
        }

        SetRefreshToken(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(CreateResponse(customer));
    }

    private void SetRefreshToken(Customer customer)
    {
        customer.SetRefreshToken(jwtTokenService.CreateRefreshToken(), jwtTokenService.GetRefreshTokenExpirationUtc());
    }

    private AuthResponse CreateResponse(Customer customer)
    {
        return new AuthResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            jwtTokenService.CreateAccessToken(customer),
            customer.RefreshToken!);
    }
}
