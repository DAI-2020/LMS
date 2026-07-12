using LMS.API.Enums.GraduationProjectEnums;

namespace LMS.API.Models
{
    public class GraduationProjectSubmission
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string ProjectName { get; set; }

        public string LeadProject { get; set; }

        public string DescriptionProject { get; set; }

        public string UploadDocumentPath { get; set; }

        public ProjectPhase CurrentPhase { get; set; }

        public User Student { get; set; }
    }
}
