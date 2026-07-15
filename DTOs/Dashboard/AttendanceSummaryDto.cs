namespace LMS.API.DTOs.Dashboard;

public class AttendanceSummaryDto
{

    public double AttendancePercentage { get; set; }
    public int PassedSessions { get; set; }
    public int TotalSessions { get; set; }
    public int AttendedSessions { get; set; }
    public int MissedSessions { get; set; }

}