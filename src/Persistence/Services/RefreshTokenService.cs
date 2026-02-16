using System.Security.Cryptography;
using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.Options;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Persistence.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repository;
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenService(IRefreshTokenRepository repository, IOptions<JwtOptions> jwtOptions)
    {
        _repository = repository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> CreateAsync(AppUser user, CancellationToken ct = default)
    {
        var token = GenerateToken();

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpirationMinutes),
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(refreshToken, ct);

        return token;
    }

    public async Task<AppUser?> ValidateAndConsumeAsync(string token, CancellationToken ct = default)
    {
        var refreshToken = await _repository.GetByTokenWithUserAsync(token, ct);

        if (refreshToken == null || refreshToken.ExpiresAtUtc <= DateTime.UtcNow)
            return null;

        // Consume: bir dəfə istifadə — silmək
        await _repository.DeleteByTokenAsync(token, ct);

        return refreshToken.User;
    }

    private static string GenerateToken()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToHexString(bytes);
    }
}
