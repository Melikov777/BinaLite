using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PropertyMediaConfiguration : IEntityTypeConfiguration<PropertyMedia>
{
    public void Configure(EntityTypeBuilder<PropertyMedia> builder)
    {
        builder.ToTable(nameof(PropertyMedia));

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.MediaUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pm => pm.MediaType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pm => pm.PropertyAdId)
            .IsRequired();
    }
}
