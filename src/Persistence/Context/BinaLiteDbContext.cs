using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class BinaLiteDbContext : IdentityDbContext<AppUser>
{
    public BinaLiteDbContext(DbContextOptions<BinaLiteDbContext> options) : base(options)
    {
    }
    
    public DbSet<PropertyAd> PropertyAds { get; set; } = null!;
    public DbSet<PropertyMedia> PropertyMedias { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BinaLiteDbContext).Assembly);
        
        // PropertyAd -> AppUser relationship
        modelBuilder.Entity<PropertyAd>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
