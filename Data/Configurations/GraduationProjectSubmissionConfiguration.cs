using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class GraduationProjectSubmissionConfiguration : IEntityTypeConfiguration<GraduationProjectSubmission>
{
    public void Configure(EntityTypeBuilder<GraduationProjectSubmission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProjectName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.LeadProject)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.DescriptionProject)
               .IsRequired()
               .HasMaxLength(2000);

        builder.Property(x => x.UploadDocumentProject)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.ProjectStatus)
               .HasConversion<string>()
               .IsRequired();

        builder.HasOne(x => x.Student)
               .WithMany(x => x.GraduationProjectSubmissions)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
