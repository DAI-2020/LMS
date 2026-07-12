using LMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.API.Data.Configurations;

public class GraduationProjectSubmissionConfiguration
    : IEntityTypeConfiguration<GraduationProjectSubmission>
{
    public void Configure(EntityTypeBuilder<GraduationProjectSubmission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProjectName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.LeadProject)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.DescriptionProject)
               .HasMaxLength(2000);

        builder.Property(x => x.UploadDocumentPath)
               .IsRequired();

        builder.Property(x => x.CurrentPhase)
               .HasConversion<string>();

        // One User -> One Graduation Project
        builder.HasOne(x => x.Student)
               .WithOne(x => x.GraduationProject)
               .HasForeignKey<GraduationProjectSubmission>(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}