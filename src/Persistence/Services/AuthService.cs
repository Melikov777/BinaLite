using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Application.Abstracts.Services;
using Application.DTOs.Auth;
using Application.Options;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly IEmailService _emailService;
    private readonly EmailOptions _emailOptions;

    public AuthService(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions,
        IEmailService emailService,
        IOptions<EmailOptions> emailOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _emailService = emailService;
        _emailOptions = emailOptions.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse { Success = false, Error = "Email already registered" };
        }

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            CreatedDate = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse { Success = false, Error = errors };
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var confirmUrl = $"{_emailOptions.ConfirmationBaseUrl}?userId={user.Id}&token={encodedToken}";

        var htmlBody = $"<h2>Email təsdiqləməsi</h2><p>Hesabınızı təsdiqləmək üçün <a href='{confirmUrl}'>buraya klikləyin</a>.</p>";
        await _emailService.SendAsync(user.Email!, "Email təsdiqləməsi", htmlBody, ct: ct);

        return new AuthResponse { Success = true, Message = "Təsdiq emaili göndərildi. Zəhmət olmasa emailinizi yoxlayın." };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse { Success = false, Error = "Invalid credentials" };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return new AuthResponse { Success = false, Error = "Invalid credentials" };
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            return new AuthResponse { Success = false, Error = "Email not confirmed" };
        }

        return await GenerateTokenResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            return new AuthResponse { Success = false, Error = "Invalid access token" };
        }

        var email = principal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email!);

        if (user == null ||
            user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new AuthResponse { Success = false, Error = "Invalid or expired refresh token" };
        }

        return await GenerateTokenResponseAsync(user);
    }

    public async Task<AuthResponse> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new AuthResponse { Success = false, Error = "User not found" };

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse { Success = false, Error = errors };
        }

        return new AuthResponse { Success = true, Message = "Email uğurla təsdiqləndi." };
    }

    private async Task<AuthResponse> GenerateTokenResponseAsync(AppUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpirationMinutes);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        await _userManager.UpdateAsync(user);

        return new AuthResponse
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiry
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}