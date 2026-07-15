using LMS.API.DTOs.Assignment;
using LMS.API.Enums.TasksEnums;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations;

public class AssignmentService : IAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public AssignmentService(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<List<AssignmentCardDto>> GetAssignmentsAsync(int studentId, AssignmentFilterDto filter)
    {
        var tasks = await _unitOfWork.CourseTasks.GetAllAsync();
        var submissions = await _unitOfWork.TaskSubmissions.GetAllAsync();
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var sessions = await _unitOfWork.LiveSessions.GetAllAsync();
        var attendanceLogs = await _unitOfWork.AttendanceLogs.GetAllAsync();

        var studentSubmissions = submissions.Where(s => s.StudentId == studentId).ToList();

        var cards = tasks.Select(task =>
        {
            var course = courses.FirstOrDefault(c => c.Id == task.CourseId);
            var session = sessions.FirstOrDefault(s => s.CourseId == task.CourseId);
            var submission = studentSubmissions.FirstOrDefault(s => s.TaskId == task.Id);
            var attendance = session is not null
                ? attendanceLogs.FirstOrDefault(a =>
                    a.StudentId == studentId && a.SessionId == session.Id)
                : null;

            var now = DateTime.UtcNow;
            var isToday = task.DueDate.Date == now.Date;
            var isPast = task.DueDate < now;

            string status;
            if (submission != null)
                status = "Completed";
            else if (isToday)
                status = "Today's task";
            else if (isPast)
                status = "Missed";
            else
                status = "Pending";

            string buttonState;
            string buttonText;
            if (submission != null)
            {
                buttonState = "Submitted";
                buttonText = "View Submission";
            }
            else if (isPast)
            {
                buttonState = "Hidden";
                buttonText = string.Empty;
            }
            else
            {
                buttonState = "SubmitNow";
                buttonText = "Submit Now";
            }

            return new AssignmentCardDto
            {
                AssignmentId = task.Id,
                Status = status,
                WeekNumber = session?.WeekNumber ?? 0,
                SessionNumber = session?.Id ?? 0,
                IsTechnical = task.TaskType == TaskType.TechTask,
                HeaderDisplay = $"Week {session?.WeekNumber ?? 0} - {course?.Title ?? string.Empty}",
                SubjectName = course?.Title ?? string.Empty,
                DueDate = task.DueDate,
                TimeDisplayText = GetTimeDisplayText(task.DueDate),
                IsAttendanceCompleted = attendance != null,
                IsTaskCompleted = submission != null,
                ButtonState = buttonState,
                ButtonText = buttonText
            };
        }).ToList();

        if (!string.IsNullOrWhiteSpace(filter.Mode))
        {
            cards = cards.Where(c =>
                sessions.Any(s => s.Id == c.SessionNumber &&
                    s.Mode.ToString().Equals(filter.Mode, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return cards;
    }

    public async Task<AssignmentDetailsDto?> GetAssignmentDetailsAsync(int assignmentId, int studentId)
    {
        var task = (await _unitOfWork.CourseTasks.GetAllAsync())
            .FirstOrDefault(t => t.Id == assignmentId);
        if (task is null) return null;

        var course = (await _unitOfWork.Courses.GetAllAsync())
            .FirstOrDefault(c => c.Id == task.CourseId);
        var submission = (await _unitOfWork.TaskSubmissions.GetAllAsync())
            .FirstOrDefault(s => s.TaskId == assignmentId && s.StudentId == studentId);

        var statusLabel = submission != null ? "Completed" :
            task.DueDate < DateTime.UtcNow ? "Missed" : "Pending";

        return new AssignmentDetailsDto
        {
            AssignmentId = task.Id,
            StatusLabel = statusLabel,
            HeaderDisplay = $"Week {task.CourseId} - {course?.Title ?? string.Empty}",
            SubjectName = course?.Title ?? string.Empty,
            Description = task.Description ?? string.Empty,
            DueDate = task.DueDate,
            TimeDisplayText = GetTimeDisplayText(task.DueDate),
            AllowedSubmissionType = "PDF",
            MaxFileSizeInMB = 10
        };
    }

    public async Task<bool> SubmitAssignmentAsync(int studentId, SubmitAssignmentRequestDto dto)
    {
        var task = (await _unitOfWork.CourseTasks.GetAllAsync())
            .FirstOrDefault(t => t.Id == dto.AssignmentId);
        if (task is null) return false;

        if (task.DueDate < DateTime.UtcNow)
            throw new InvalidOperationException("Submission deadline has passed.");

        var allowedExtensions = new[] { ".pdf", ".docx", ".doc" };
        var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException($"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}.");

        if (dto.File.Length > 10 * 1024 * 1024)
            throw new InvalidOperationException("File size must not exceed 10MB.");

        var fileUrl = await _fileService.UploadSubmissionAsync(dto.File);

        var submission = new Models.TaskSubmission
        {
            TaskId = dto.AssignmentId,
            StudentId = studentId,
            FileUrl = fileUrl,
            SubmittedAt = DateTime.UtcNow,
            AssignmentStatus = AssignmentStatus.Submitted
        };

        await _unitOfWork.TaskSubmissions.AddAsync(submission);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static string GetTimeDisplayText(DateTime dueDate)
    {
        var diff = dueDate - DateTime.UtcNow;
        if (diff.TotalDays > 1)
            return $"Due in {diff.Days} days";
        if (diff.TotalHours > 1)
            return $"Due in {diff.Hours} hours";
        if (diff.TotalMinutes > 0)
            return $"Due in {diff.Minutes} minutes";
        return "Overdue";
    }
}
