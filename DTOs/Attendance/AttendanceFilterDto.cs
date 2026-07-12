namespace LMS.API.DTOs.Attendance
{
    public class AttendanceFilterDto
    {
        // الفلترة بنوع المحاضرة: Technical أو NonTechnical
        public string? SessionType { get; set; }

        // الفلترة بالفترة الزمنية: ThisMonth أو ThisYear
        public string? TimeRange { get; set; }
    }
}
