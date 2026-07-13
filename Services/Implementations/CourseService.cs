using LMS.API.DTOs.Course;
using LMS.API.DTOs.Material;
using LMS.API.Repositories.Interfaces;
using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();

            return courses.Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                InstructorId = c.InstructorId
            });
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            if (course == null)
                return null;

            return new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId
            };
        }

        public async Task AddAsync(CreateCourseDto dto)
        {
            var course = new Models.Course
            {
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = dto.InstructorId
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCourseDto dto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found.");

            course.Title = dto.Title;
            course.Description = dto.Description;

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found.");

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MaterialResponseDto>> GetMaterialsBySessionAsync(int sessionId)
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();

            return materials
                .Where(m => m.SessionId == sessionId)
                .Select(m => new MaterialResponseDto
                {
                    Id = m.Id,
                    CourseId = m.CourseId,
                    SessionId = m.SessionId,
                    Title = m.Title,
                    MaterialType = m.MaterialType,
                    AttachmentType = m.AttachmentType,
                    FileUrl = m.FileUrl
                });
        }
    }
}
