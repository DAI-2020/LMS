namespace LMS.API.Services.Implementations;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadMaterialAsync(IFormFile file)
    {
        return await SaveFileAsync(file, "materials");
    }

    public async Task<string> UploadSubmissionAsync(IFormFile file)
    {
        return await SaveFileAsync(file, "submissions");
    }

    private async Task<string> SaveFileAsync(IFormFile file, string subFolder)
    {
        var rootPath = _environment.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var uploadsFolder = Path.Combine(rootPath, "uploads", subFolder);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{subFolder}/{uniqueName}";
    }
}
