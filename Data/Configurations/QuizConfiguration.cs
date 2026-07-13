using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score)
               .IsRequired();

        builder.HasOne(x => x.Course)
               .WithMany()
               .HasForeignKey(x => x.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Topic)
               .WithMany(x => x.Quizzes)
               .HasForeignKey(x => x.TopicId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Student)
               .WithMany(x => x.Quizzes)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
