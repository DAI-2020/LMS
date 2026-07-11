namespace LMS.API.DTOs.Attendance
{
    public class LeaveSessionDto
    {
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public int MicrophoneUsageSeconds { get; set; }
    }
}
