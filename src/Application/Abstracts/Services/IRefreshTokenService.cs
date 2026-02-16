using Domain.Entities;

namespace Application.Abstracts.Services;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(AppUser user, CancellationToken ct = default);
    Task<AppUser?> ValidateAndConsumeAsync(string token, CancellationToken ct = default);
}
