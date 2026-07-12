using LMS.API.Enums.GraduationProjectEnums;
using LMS.API.Models;

namespace LMS.API.Repositories.Interfaces;

public interface IGraduationProjectRepository : IRepository<GraduationProjectSubmission>
{
    Task<GraduationProjectSubmission?> GetByStudentIdAsync(int studentId);
}