using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class TicketReplyConfiguration : IEntityTypeConfiguration<TicketReply>
    {
        public void Configure(EntityTypeBuilder<TicketReply> builder)
        {
            builder.HasKey(tr => tr.Id);

            // 5. العلاقة بين TicketReply و Ticket
            builder.HasOne(tr => tr.Ticket)
                   .WithMany(t => t.TicketReplies)
                   .HasForeignKey(tr => tr.TicketId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 6. العلاقة بين TicketReply و User (Replier)
            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
