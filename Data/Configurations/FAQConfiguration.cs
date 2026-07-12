using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations
{
    public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Question)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(f => f.Answer)
                   .IsRequired()
                   .HasMaxLength(2000);
        }
    }
}
