using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(x => x.Email)
               .IsUnique();

        builder.Property(x => x.PasswordHash)
               .IsRequired();
    }
}