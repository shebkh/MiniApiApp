using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
{
    public void Configure(EntityTypeBuilder<Organizer> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Email).IsRequired().HasMaxLength(255);
        builder.Property(o => o.Phone).HasMaxLength(20);
        builder.HasIndex(o => o.Email).IsUnique(); // emails must be unique
    }
}

