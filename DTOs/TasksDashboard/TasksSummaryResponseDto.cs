namespace LMS.API.DTOs.TasksDashboard;

public class TasksSummaryResponseDto
{
    public int TotalTasks { get; set; }

    public int CompletedTasks { get; set; }

    public int ReviewedTasks { get; set; }

    public int MissedTasks { get; set; }

    public int MaxTasksAtSession { get; set; }
}
