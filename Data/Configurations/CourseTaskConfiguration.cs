using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class CourseTaskConfiguration : IEntityTypeConfiguration<Models.CourseTask>
{
    public void Configure(EntityTypeBuilder<Models.CourseTask> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired();

        builder.HasOne(x => x.Course)
               .WithMany(x => x.CourseTasks)
               .HasForeignKey(x => x.CourseId);

        builder.HasOne(t => t.LiveSession)
               .WithMany() // أو WithMany(s => s.Tasks) لو حابب تضيفها في جدولك
               .HasForeignKey(t => t.SessionId)
               .OnDelete(DeleteBehavior.SetNull); // SetNull عشان لو المحاضرة اتمسحت الواجب ميتسحش من الكورس
        
        builder.Property(x => x.Description)
       .HasMaxLength(1000);
        
        builder.Property(m => m.TaskType).HasConversion<string>();
        builder.Property(m => m.AssignmentStatus).HasConversion<string>();
    }
}