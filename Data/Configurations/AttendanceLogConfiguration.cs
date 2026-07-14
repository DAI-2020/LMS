using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
    {
        public void Configure(EntityTypeBuilder<AttendanceLog> builder)
        {
            builder.HasKey(al => al.Id);

            // 2. العلاقة بين AttendanceLog و LiveSession
            builder.HasOne(al => al.LiveSession)
                .WithMany(ls => ls.AttendanceLogs)
                .HasForeignKey(al => al.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. العلاقة بين AttendanceLog و User (الطالب)
            builder.HasOne(a => a.Student)
               .WithMany(u => u.AttendanceLogs)
               .HasForeignKey(a => a.StudentId)
               .OnDelete(DeleteBehavior.Restrict); // منعا للـ Multiple Cascade Paths

            // حفظ الـ Enum كـ String
            builder.Property(a => a.ParticipationLevel)
               .HasConversion<string>();

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

        }
    }
}
