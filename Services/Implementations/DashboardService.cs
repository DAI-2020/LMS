using LMS.API.DTOs.Dashboard;
using LMS.API.DTOs.LiveSession;
using LMS.API.DTOs.Attendance;
using LMS.API.Enums.AttendanceLog;
using LMS.API.Enums.TasksEnums;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAttendanceSummaryService _attendanceSummaryService;
    private readonly ILiveSessionService _liveSessionService;

    public DashboardService(
        IUnitOfWork unitOfWork,
        IAttendanceSummaryService attendanceSummaryService,
        ILiveSessionService liveSessionService)
    {
        _unitOfWork = unitOfWork;
        _attendanceSummaryService = attendanceSummaryService;
        _liveSessionService = liveSessionService;
    }

    public async Task<TasksSummaryDto> GetTasksSummaryAsync(int studentId)
    {
        var allTasks = await _unitOfWork.CourseTasks.GetAllAsync();
        var allSubmissions = await _unitOfWork.TaskSubmissions.GetAllAsync();

        var studentSubmissions = allSubmissions
            .Where(s => s.StudentId == studentId)
            .ToList();

        var totalTasks = allTasks.Count();
        var completedTasks = studentSubmissions.Count(s => s.AssignmentStatus == AssignmentStatus.Submitted);
        var reviewedTasks = studentSubmissions.Count(s => s.AssignmentStatus == AssignmentStatus.Reviewed);
        var missedTasks = allTasks.Count(t =>
            t.DueDate < DateTime.UtcNow &&
            !studentSubmissions.Any(s => s.TaskId == t.Id));
        var pendingTasks = totalTasks - completedTasks - reviewedTasks - missedTasks;
        if (pendingTasks < 0) pendingTasks = 0;

        return new TasksSummaryDto
        {
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            ReviewedTasks = reviewedTasks,
            MissedTasks = missedTasks,
            PendingTasks = pendingTasks
        };
    }

    public async Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(int studentId)
    {
        var summary = await _attendanceSummaryService.GetAttendanceSummaryAsync(
            studentId, new AttendanceFilterDto());

        return new AttendanceSummaryDto
        {
            TotalSessions = summary.TotalSessions,
            AttendedSessions = summary.AttendedSessions,
            MissedSessions = summary.MissedSessions,
            AttendancePercentage = summary.AttendancePercentage
        };
    }

    public async Task<IEnumerable<ActiveSessionsSummaryDto>> GetActiveSessionsAsync()
    {
        var todaySessions = await _liveSessionService.GetTodaySessionsAsync();
        var cards = todaySessions.Select(s => new LiveSessionCardDto
        {
            Id = s.Id,
            Title = s.Title,
            CourseName = s.CourseName,
            Status = s.Status,
            Type = s.Type,
            Mode = s.Mode,
            ScheduledAt = s.ScheduledAt,
            DurationMinutes = s.DurationMinutes,
            HasAttendance = s.HasAttendance,
            HasTask = s.HasTask,
            HasSurvey = s.HasSurvey,
            RecordingUrl = s.RecordingUrl,
            ActionLabel = s.ActionLabel ?? "Join Session",
            AttendanceCount = s.AttendanceCount
        }).ToList();

        var result = new ActiveSessionsSummaryDto
        {
            LiveNowCount = cards.Count(c => c.Status == "Live"),
            UpcomingTodayCount = cards.Count(c => c.Status == "TodaySession" || c.Status == "Upcoming"),
            TodaySessions = cards
        };

        return new List<ActiveSessionsSummaryDto> { result };
    }

    public async Task<IEnumerable<GrowthAreaMetricDto>> GetGrowthAreasAsync(int studentId)
    {
        var allQuizzes = await _unitOfWork.Quizzes.GetAllWithTopicAsync();

        var topicScores = allQuizzes
            .GroupBy(q => q.TopicId)
            .Select(g => new
            {
                TopicId = g.Key,
                TopicName = g.First().Topic.Name,
                AverageScore = g.Average(q => q.Score)
            })
            .Where(t => t.AverageScore < 60)
            .ToList();

        if (!topicScores.Any())
            return Enumerable.Empty<GrowthAreaMetricDto>();

        var allStudentScores = allQuizzes
            .GroupBy(q => new { q.StudentId, q.TopicId })
            .Select(g => new
            {
                g.Key.StudentId,
                g.Key.TopicId,
                AverageScore = g.Average(q => q.Score)
            })
            .ToList();

        var result = new List<GrowthAreaMetricDto>();

        foreach (var topic in topicScores)
        {
            var topicStudentRankings = allStudentScores
                .Where(s => s.TopicId == topic.TopicId)
                .OrderByDescending(s => s.AverageScore)
                .ToList();

            var studentRank = topicStudentRankings.FindIndex(s => s.StudentId == studentId) + 1;
            var totalStudents = topicStudentRankings.Count;
            var ordinal = GetOrdinal(studentRank);

            result.Add(new GrowthAreaMetricDto
            {
                Topic = topic.TopicName,
                Score = Math.Round(topic.AverageScore, 2),
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
