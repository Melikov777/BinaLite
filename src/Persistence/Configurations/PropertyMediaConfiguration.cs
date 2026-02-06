using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
{
    public void Configure(EntityTypeBuilder<PropertyMedia> builder)
    {
        builder.ToTable("PropertyMedias");

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.ObjectKey)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pm => pm.Order)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(pm => pm.PropertyAd)
            .WithMany(ad => ad.MediaItems)
            .HasForeignKey(pm => pm.PropertyAdId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pm => pm.PropertyAdId);
        builder.HasIndex(pm => new { pm.PropertyAdId, pm.Order });
    }
}
