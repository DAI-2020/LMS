using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            // 4. العلاقة بين Ticket و User (Student)
            builder.HasOne(t => t.Student)
                .WithMany()
                .HasForeignKey(t => t.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. العلاقة بين Ticket و TicketReply
            builder.HasMany(t => t.TicketReplies)
                .WithOne(tr => tr.Ticket)
                .HasForeignKey(tr => tr.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // تحويل الـ Enums الخاصة بالتذكرة لـ Strings
            builder.Property(t => t.Category)
                .HasConversion<string>();

            builder.Property(t => t.Priority)
                .HasConversion<string>();

            builder.Property(t => t.Status)
                .HasConversion<string>();
        }
    }
}
