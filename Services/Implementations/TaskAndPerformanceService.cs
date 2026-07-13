using LMS.API.DTOs.GrowthAreas;
using LMS.API.DTOs.TasksDashboard;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations;

public class TaskAndPerformanceService : ITaskAndPerformanceService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskAndPerformanceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TasksSummaryResponseDto> GetTasksSummaryAsync(int studentId, int courseId)
    {
        var tasks = await _unitOfWork.CourseTasks.GetAllAsync();
        var courseTasks = tasks.Where(t => t.CourseId == courseId).ToList();

        var submissions = await _unitOfWork.TaskSubmissions.GetAllAsync();
        var studentSubmissions = submissions
            .Where(s => s.StudentId == studentId && courseTasks.Any(t => t.Id == s.TaskId))
            .ToList();

        var totalTasks = courseTasks.Count;
        var completedTasks = studentSubmissions.Count(s => s.AssignmentStatus == Enums.TasksEnums.AssignmentStatus.Submitted);
        var reviewedTasks = studentSubmissions.Count(s => s.AssignmentStatus == Enums.TasksEnums.AssignmentStatus.Reviewed);
        var missedTasks = courseTasks.Count(t =>
            t.DueDate < DateTime.UtcNow &&
            !studentSubmissions.Any(s => s.TaskId == t.Id));

        var maxTasksAtSession = courseTasks.Any()
            ? courseTasks.GroupBy(t => t.SessionId ?? 0)
                         .Max(g => g.Count())
            : 0;

        return new TasksSummaryResponseDto
        {
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            ReviewedTasks = reviewedTasks,
            MissedTasks = missedTasks,
            MaxTasksAtSession = maxTasksAtSession
        };
    }

    public async Task<IEnumerable<GrowthAreaResponseDto>> GetGrowthAreasAsync(int studentId, int courseId)
    {
        var quizzes = await _unitOfWork.Quizzes.GetByCourseIdAsync(courseId);

        var topicScores = quizzes
            .GroupBy(q => q.TopicId)
            .Select(g => new
            {
                TopicId = g.Key,
                TopicName = g.First().Topic.Name,
                AverageScore = g.Average(q => q.Score)
            })
            .Where(t => t.AverageScore < 60)
            .ToList();

        var allStudentScores = quizzes
            .GroupBy(q => new { q.StudentId, q.TopicId })
            .Select(g => new
            {
                g.Key.StudentId,
                g.Key.TopicId,
                AverageScore = g.Average(q => q.Score)
            })
            .ToList();

        var result = new List<GrowthAreaResponseDto>();

        foreach (var topic in topicScores)
        {
            var topicStudentRankings = allStudentScores
                .Where(s => s.TopicId == topic.TopicId)
                .OrderByDescending(s => s.AverageScore)
                .ToList();

            var studentRank = topicStudentRankings.FindIndex(s => s.StudentId == studentId) + 1;
            var totalStudents = topicStudentRankings.Count;

            var ordinal = GetOrdinal(studentRank);

            result.Add(new GrowthAreaResponseDto
            {
                Topic = topic.TopicName,
                ProficiencyRate = Math.Round(topic.AverageScore, 2),
                Ranking = $"{ordinal} out of {totalStudents} students"
            });
        }

        return result;
    }

    private static string GetOrdinal(int number)
    {
        if (number <= 0) return number.ToString();

        var suffix = (number % 100) switch
        {
            11 or 12 or 13 => "th",
            _ => (number % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            }
        };

        return $"{number}{suffix}";
    }
}
