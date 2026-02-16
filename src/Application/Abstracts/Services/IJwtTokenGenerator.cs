using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Abstracts.Services;

public interface IJwtTokenGenerator
{
    TokenResponse GenerateAccessToken(AppUser user);
}