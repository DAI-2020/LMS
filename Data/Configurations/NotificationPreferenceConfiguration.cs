using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreferences>
    {
        public void Configure(EntityTypeBuilder<NotificationPreferences> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.User)
                   .WithMany(u => u.NotificationPreferences)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(n => n.NewLessons)
                   .HasDefaultValue(true);

            builder.Property(n => n.LiveSessionReminders)
                   .HasDefaultValue(true);

            builder.Property(n => n.AssignmentDeadlines)
                   .HasDefaultValue(true);

            builder.Property(n => n.AssignmentGrading)
                   .HasDefaultValue(true);

            builder.Property(n => n.QuizAlerts)
                   .HasDefaultValue(true);

            builder.Property(n => n.CommunityNotifications)
                   .HasDefaultValue(true);

            builder.Property(n => n.AiRecommendations)
                   .HasDefaultValue(false);

            builder.Property(n => n.SecurityAlerts)
                   .HasDefaultValue(true);
        }
    }
}
