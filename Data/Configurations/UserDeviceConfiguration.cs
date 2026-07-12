using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.ToTable("user_devices");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DeviceName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(d => d.ClientInfo)
                   .HasMaxLength(250);

            builder.Property(d => d.RefreshTokenHash)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.LastUsed)
                   .IsRequired();

            // إعداد العلاقة One-to-Many مع جدول الـ User (المسؤول عنه زميلك)
            builder.HasOne(d => d.User)
                   .WithMany(u => u.UserDevices) // تأكد إن زميلك هيفضل الـ Collection دي جوه الـ User Entity
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // حذف الأجهزة تلقائياً عند حذف المستخدم
        }
    }
}
