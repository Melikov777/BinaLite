using Application.Abstracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly BinaLiteDbContext _context;

    public RefreshTokenRepository(BinaLiteDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken ct = default)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, ct);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteByTokenAsync(string token, CancellationToken ct = default)
    {
        await _context.RefreshTokens
            .Where(rt => rt.Token == token)
            .ExecuteDeleteAsync(ct);
    }
}
