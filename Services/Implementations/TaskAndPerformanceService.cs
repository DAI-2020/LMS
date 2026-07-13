using LMS.API.DTOs.Dashboard;
using LMS.API.Enums.TasksEnums;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services.Implementations
{
    public class TaskAndPerformanceService : ITaskAndPerformanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskAndPerformanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TasksSummaryResponseDto> GetTasksSummaryAsync(int courseId)
        {
            var tasks = (await _unitOfWork.CourseTasks.GetAllAsync())
                .Where(t => t.CourseId == courseId)
                .ToList();

            var allSubmissions = (await _unitOfWork.TaskSubmissions.GetAllAsync()).ToList();

            var totalTasks = tasks.Count;

            var completedTasks = allSubmissions.Count(s =>
                s.AssignmentStatus == AssignmentStatus.Submitted);

            var reviewedTasks = allSubmissions.Count(s =>
                s.AssignmentStatus == AssignmentStatus.Reviewed);

            var missedTasks = tasks.Count(t =>
                t.AssignmentStatus == AssignmentStatus.Missed);

            var maxTasksAtSession = tasks
                .GroupBy(t => t.SessionId)
                .Select(g => g.Count())
                .DefaultIfEmpty(0)
                .Max();

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
            var topics = (await _unitOfWork.Topics.GetTopicsByCourseAsync(courseId)).ToList();

            var growthAreas = new List<GrowthAreaResponseDto>();

            foreach (var topic in topics)
            {
                var quizzes = (await _unitOfWork.Quizzes.GetByTopicAsync(topic.Id))
                    .Where(q => q.StudentId == studentId)
                    .ToList();

                if (!quizzes.Any())
                    continue;

                var latestQuiz = quizzes.OrderByDescending(q => q.TakenAt).First();
                var proficiencyRate = latestQuiz.TotalScore > 0
                    ? (latestQuiz.Score / latestQuiz.TotalScore) * 100
                    : 0;

                if (proficiencyRate >= 60)
                    continue;

                var allStudentsInCourseQuizzes = new List<double>();
                var allCourseTopics = (await _unitOfWork.Topics.GetTopicsByCourseAsync(courseId)).ToList();

                foreach (var courseTopic in allCourseTopics)
                {
                    var topicQuizzes = (await _unitOfWork.Quizzes.GetByTopicAsync(courseTopic.Id)).ToList();
                    var studentIds = topicQuizzes.Select(q => q.StudentId).Distinct();

                    foreach (var sid in studentIds)
                    {
                        var studentScore = topicQuizzes
                            .Where(q => q.StudentId == sid)
                            .OrderByDescending(q => q.TakenAt)
                            .First();

                        if (studentScore.TotalScore > 0)
                        {
                            allStudentsInCourseQuizzes.Add((studentScore.Score / studentScore.TotalScore) * 100);
                        }
                    }
                }

                var betterCount = allStudentsInCourseQuizzes.Count(s => s > proficiencyRate);
                var rank = betterCount + 1;
                var totalStudents = allStudentsInCourseQuizzes.Count;

                growthAreas.Add(new GrowthAreaResponseDto
                {
                    TopicName = topic.Name,
                    ProficiencyRate = Math.Round(proficiencyRate, 2),
                    Rank = $"{rank}st out of {totalStudents} students"
                });
            }

            return growthAreas.OrderByDescending(g => g.ProficiencyRate);
        }
    }
}
