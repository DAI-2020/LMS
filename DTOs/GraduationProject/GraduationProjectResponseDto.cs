namespace LMS.API.DTOs.GraduationProject
{
    public class GraduationProjectResponseDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string ProjectName { get; set; }

        public string LeadProject { get; set; }

        public string DescriptionProject { get; set; }

        public string UploadDocumentPath { get; set; }

        public string CurrentPhase { get; set; }
    }
}
