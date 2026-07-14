namespace LMS.API.DTOs.Dashboard;

public class AttendanceSummaryDto
{
    public int TotalSessions { get; set; }
    public int AttendedSessions { get; set; }
    public int MissedSessions { get; set; }
    public double AttendancePercentage { get; set; }
}
