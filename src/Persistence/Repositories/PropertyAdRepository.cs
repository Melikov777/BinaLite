using Application.Abstracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class PropertyAdRepository : GenericRepository<PropertyAd, int>, IPropertyAdRepository
{
    private readonly BinaLiteDbContext _context;

    public PropertyAdRepository(BinaLiteDbContext context) : base(context)
    {
        _context = context;
    }

}