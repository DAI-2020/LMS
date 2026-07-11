using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
{
    public void Configure(EntityTypeBuilder<TaskSubmission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Task)
               .WithMany(x => x.Submissions)
               .HasForeignKey(x => x.TaskId);

        builder.HasOne(x => x.Student)
               .WithMany(x => x.TaskSubmissions)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.AssignmentStatus)
        .HasConversion<string>();

    }
}