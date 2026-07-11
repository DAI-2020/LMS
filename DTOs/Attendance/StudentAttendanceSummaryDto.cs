namespace LMS.API.DTOs.Attendance
{
    public class StudentAttendanceSummaryDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int TotalSessions { get; set; }
        public int AttendedSessions { get; set; }
        public int MissedSessions { get; set; }
        public int PassedSessions { get; set; }
        public double AttendancePercentage { get; set; }
        public double AverageEngagementScore { get; set; }
    }
}
