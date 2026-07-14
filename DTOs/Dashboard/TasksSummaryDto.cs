namespace LMS.API.DTOs.Dashboard;

public class TasksSummaryDto
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int ReviewedTasks { get; set; }
    public int MissedTasks { get; set; }
    public int PendingTasks { get; set; }
}
