using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations;

public class TaskSubmissionRepository : Repository<TaskSubmission>, ITaskSubmissionRepository
{
    public TaskSubmissionRepository(LMSDbContext context)
        : base(context)
    {
    }
}