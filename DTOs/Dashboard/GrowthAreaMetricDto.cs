namespace LMS.API.DTOs.Dashboard;

public class GrowthAreaMetricDto
{
    public string Topic { get; set; } = string.Empty;
    public double Score { get; set; }
    public string Ranking { get; set; } = string.Empty;
}
