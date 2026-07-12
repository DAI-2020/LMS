using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class LiveSessionConfigurations : IEntityTypeConfiguration<LiveSession>
    {
        public void Configure(EntityTypeBuilder<LiveSession> builder)
        {
            builder.HasKey(ls => ls.Id);

            builder.Property(ls => ls.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.WeekNumber)
                .IsRequired();

            // 1. العلاقة بين LiveSession و Course
            builder.HasOne(ls => ls.Course)
                .WithMany(c => c.LiveSessions)
                .HasForeignKey(ls => ls.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            //حفظ الـ Enum كـ String في الداتا بيز
            builder.Property(ls => ls.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ls => ls.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ls => ls.Mode)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
