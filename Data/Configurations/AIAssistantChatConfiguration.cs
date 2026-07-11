using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class AIAssistantChatConfiguration : IEntityTypeConfiguration<AIAssistantChat>
{
    public void Configure(EntityTypeBuilder<AIAssistantChat> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserQuery)
               .IsRequired();

        builder.Property(x => x.AIResponse)
               .IsRequired();

        builder.HasOne(x => x.Student)
               .WithMany(x => x.Chats)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Course)
               .WithMany(x => x.Chats)
               .HasForeignKey(x => x.CourseId);
    }
}