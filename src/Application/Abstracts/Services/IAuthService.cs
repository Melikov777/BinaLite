using Application.DTOs.Auth;

namespace Application.Abstracts.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
    Task<AuthResponse> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default);
}