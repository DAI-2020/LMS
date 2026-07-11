using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasOne(x => x.Instructor)
               .WithMany(x => x.Courses)
               .HasForeignKey(x => x.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.Description)
       .HasMaxLength(500);
    }
}