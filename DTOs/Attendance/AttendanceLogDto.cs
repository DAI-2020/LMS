namespace LMS.API.DTOs.Attendance
{
    public class AttendanceLogDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime? JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
        public string Status { get; set; }
        public string ParticipationLevel { get; set; }
        public int EngagementScore { get; set; }
        public int MicrophoneUsageSeconds { get; set; }
    }
}
