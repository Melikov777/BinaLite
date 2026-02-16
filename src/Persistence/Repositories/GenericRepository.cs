using Application.Abstracts.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class GenericRepository<Tentity, Tkey> : IRepository<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
{
    private readonly BinaLiteDbContext _context;
    private readonly DbSet<Tentity> _dbSet;

    public GenericRepository(BinaLiteDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Tentity>();
    }

    public async Task<List<Tentity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<Tentity?> GetByIdAsync(Tkey id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id! }, ct);
    }

    public async Task AddAsync(Tentity entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
    }

    public void Update(Tentity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Tentity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}