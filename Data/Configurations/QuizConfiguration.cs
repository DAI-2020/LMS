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

        builder.Property(x => x.TotalScore)
               .IsRequired();

        builder.Property(x => x.TakenAt)
               .IsRequired();

        builder.HasOne(x => x.Topic)
               .WithMany(x => x.Quizzes)
               .HasForeignKey(x => x.TopicId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Student)
               .WithMany(x => x.Quizzes)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}