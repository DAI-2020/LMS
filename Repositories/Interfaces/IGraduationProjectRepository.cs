using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface IGraduationProjectRepository : IRepository<GraduationProjectSubmission>
{
    Task<IEnumerable<GraduationProjectSubmission>> GetByStudentIdAsync(int studentId);
}
