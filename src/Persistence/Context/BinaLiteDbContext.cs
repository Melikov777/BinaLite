using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class BinaLiteDbContext: DbContext
{
    public BinaLiteDbContext(DbContextOptions<BinaLiteDbContext> options) : base(options)
    {
    }
    
    public DbSet<PropertyAd> PropertyAds { get; set; } = null!;
    public DbSet<PropertyMedia> PropertyMedias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BinaLiteDbContext).Assembly);
    }
}
