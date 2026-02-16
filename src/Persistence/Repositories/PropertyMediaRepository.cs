using Application.Abstracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class PropertyMediaRepository : GenericRepository<PropertyMedia, int>, IPropertyMediaRepository
{
    private readonly BinaLiteDbContext _context;

    public PropertyMediaRepository(BinaLiteDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<PropertyMedia>> GetByPropertyAdIdAsync(int propertyAdId, CancellationToken ct = default)
    {
        return await _context.Set<PropertyMedia>()
            .Where(pm => pm.PropertyAdId == propertyAdId)
            .OrderBy(pm => pm.Order)
            .ToListAsync(ct);
    }
}