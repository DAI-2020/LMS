using LMS.API.Services.Interfaces;

namespace LMS.API.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadMaterialAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded.");

            var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads", "materials");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/materials/{fileName}";
        }

        public async Task<string> UploadSubmissionAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded.");

            var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads", "submissions");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/submissions/{fileName}";
        }
    }
}
