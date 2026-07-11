using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired();

        builder.HasOne(x => x.Course)
               .WithMany(x => x.Materials)
               .HasForeignKey(x => x.CourseId);

        builder.HasOne(m => m.LiveSession)
           .WithMany()
           .HasForeignKey(m => m.SessionId)
           .OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.MaterialType).HasConversion<string>();
        builder.Property(m => m.AttachmentType).HasConversion<string>();

    }
}